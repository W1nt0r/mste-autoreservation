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
    public class AutoManager
        : ManagerBase
    {

        public List<Auto> List
        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Autos.Include(r => r.Reservationen).ToList();
                }
            }
        }

        public Auto Auto(int autoId)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Auto auto = context.Autos.Include(r => r.Reservationen).SingleOrDefault(a => a.Id == autoId);
                if (auto != default(Auto))
                {
                    return auto;
                }
                throw new EntityNotFoundException($"Could not find auto with id {autoId}");
            }
        }

        public Auto Insert(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Autos.Add(auto);
                context.SaveChanges();
                if (context.Entry(auto).State == EntityState.Unchanged)
                {
                    return auto;
                }
                throw new DatabaseChangeException("Could not insert auto");
            }
        }

        public Auto Update(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Auto autoFromList = context.Autos.SingleOrDefault(a => a.Id == auto.Id);
                if (autoFromList != default(Auto))
                {
                    context.Entry(autoFromList).OriginalValues["RowVersion"] = auto.RowVersion;
                    autoFromList.CopyFrom(auto);
                    try
                    {
                        context.SaveChanges();
                        if (context.Entry(autoFromList).State == EntityState.Unchanged)
                        {
                            return context.Autos.Include(r => r.Reservationen).SingleOrDefault(a => a.Id == auto.Id);
                        }
                        throw new DatabaseChangeException("Could not update auto");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw CreateOptimisticConcurrencyException(context, autoFromList);
                    }                    
                }
                throw new EntityNotFoundException($"Could not find auto with id {auto.Id}");
            }
        }

        public Auto Delete(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Auto autoFromList = context.Autos.Include(r => r.Reservationen).SingleOrDefault(a => a.Id == auto.Id);
                if (autoFromList != default(Auto))
                {
                    context.Entry(autoFromList).OriginalValues["RowVersion"] = auto.RowVersion;
                    context.Autos.Remove(autoFromList);
                    try
                    {
                        context.SaveChanges();
                        if (context.Entry(autoFromList).State == EntityState.Detached)
                        {
                            return autoFromList;
                        }
                        throw new DatabaseChangeException("Could not delete auto");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw CreateOptimisticConcurrencyException(context, autoFromList);
                    }
                }
                throw new EntityNotFoundException($"Could not find auto with id {auto.Id}");
            }
        }
        
    }
}