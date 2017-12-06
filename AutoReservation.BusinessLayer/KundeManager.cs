using System;
using AutoReservation.Dal;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public List<Kunde> List
        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Kunden.Include(r => r.Reservationen).ToList();
                }
            }
        }

        public Kunde Kunde(int kundenId)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Kunde kunde = context.Kunden.Include(r => r.Reservationen).SingleOrDefault(k => k.Id == kundenId);
                if (kunde != default(Kunde))
                {
                    return kunde;
                }
                throw new EntityNotFoundException($"Could not find kunde with id {kundenId}");
            }
        }

        public Kunde Insert(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Kunden.Add(kunde);
                context.SaveChanges();
                if (context.Entry(kunde).State == EntityState.Unchanged)
                {
                    return kunde;
                }
                throw new DatabaseChangeException("Could not insert kunde");
            }
        }

        public Kunde Update(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Kunde kundeFromList = context.Kunden.SingleOrDefault(k => k.Id == kunde.Id);
                if (kundeFromList != default(Kunde))
                {
                    context.Entry(kundeFromList).OriginalValues["RowVersion"] = kunde.RowVersion;
                    kundeFromList.CopyFrom(kunde);
                    try
                    {
                        context.SaveChanges();
                        if (context.Entry(kundeFromList).State == EntityState.Unchanged)
                        {
                            return context.Kunden.Include(r => r.Reservationen).SingleOrDefault(k => k.Id == kunde.Id);
                        }
                        throw new DatabaseChangeException("Could not update kunde");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw CreateOptimisticConcurrencyException(context, kundeFromList);
                    }
                    
                }
                throw new EntityNotFoundException($"Could not find kunde with id {kunde.Id}");
            }
        }

        public Kunde Delete(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Kunde kundeFromList = context.Kunden.Include(r => r.Reservationen).SingleOrDefault(k => k.Id == kunde.Id);
                if (kundeFromList != default(Kunde))
                {
                    context.Entry(kundeFromList).OriginalValues["RowVersion"] = kunde.RowVersion;
                    context.Kunden.Remove(kundeFromList);
                    try
                    {
                        context.SaveChanges();
                        if (context.Entry(kundeFromList).State == EntityState.Detached)
                        {
                            return kundeFromList;
                        }
                        throw new DatabaseChangeException("Could not delete kunde");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw CreateOptimisticConcurrencyException(context, kundeFromList);
                    }                    
                }
                throw new EntityNotFoundException($"Could not find kunde with id {kunde.Id}");
            }
        }
    }
}