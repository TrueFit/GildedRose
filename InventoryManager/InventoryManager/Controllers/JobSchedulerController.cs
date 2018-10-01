using Quartz;
using Quartz.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace InventoryManager.Controllers
{
    public class JobSchedulerController : Controller
    {
        public InventoryManagerEntities db = new InventoryManagerEntities();

        public async Task<ActionResult> Start()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<UpdateJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateJob ", "GreetingGroup")
                // run every 13th hour of every day
                .WithDailyTimeIntervalSchedule(s => s
                  .OnEveryDay()
                  .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(13, 00))
                  .EndingDailyAfterCount(1))
                .StartAt(DateTime.UtcNow)
                .WithPriority(1)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            return View();
        }

        /// <summary>
        /// SimpleJOb is just a class that implements IJOB interface. It implements just one method, Execute method
        /// </summary>
        public class UpdateJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                JobSchedulerController JS = new JobSchedulerController();
                bool run = JS.DailyUpdateAlgorithm(false);
            }
        }

        // Schedule Job
        public void ScheduleJob()
        {
            // construct a scheduler factory
            ISchedulerFactory sf = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            IScheduler sched = (IScheduler)sf.GetScheduler();
            sched.Start();

            // create job
            IJobDetail job = JobBuilder.Create<UpdateJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

            // create trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
                .Build();

            // Schedule the job using the job and trigger 
            sched.ScheduleJob(job, trigger);
        }

        // logic to update quality on all items in all stores
        public bool DailyUpdateAlgorithm(bool manualRun)
        {
            // get the date of the last run
            tbl_DailyUpdateLog log = db.tbl_DailyUpdateLog.OrderByDescending(x => x.DateTime).FirstOrDefault();

            // check to only run once per day
            if (log == null || log.DateTime <= DateTime.Now.AddDays(-1))
            {
                // get trash items for this store only
                var tbl_Items = db.tbl_Items.Where(x => x.Active == true && x.Quality > 0);

                foreach (tbl_Items item in tbl_Items.ToList())
                {
                    if (item.Category == "Conjured" || DateTime.Now.AddDays(item.SellIn) < DateTime.Now)
                        // SellIn < Today OR Conjured then Quality – 2
                        item.Quality = item.Quality - 2;
                    else if (item.BetterWithAge == true)
                        // BetterWithAge “Aged Brie” THEN Quality +1
                        item.Quality = item.Quality + 1;
                    else if (item.Category == "Backstage Passes")
                    {
                        // BackstagePasses WHEN SellIn <= 10 THEN Quality +2, WHEN SellIn <= 5 THEN Quality +3, WHEN SellIn = 0 THEN Quality = 0
                        if (item.SellIn <= 10)
                            item.Quality = item.Quality + 2;
                        else if (item.SellIn <= 5)
                            item.Quality = item.Quality + 3;
                        else if (item.SellIn == 0)
                            item.Quality = 0;
                    }
                    else
                    {
                        if (item.Legendary == false)
                        {
                            // otherwise lower Quality - 1
                            item.Quality = item.Quality - 1;
                        }
                    }

                    // SellIn - 1 for all except Legendary
                    if (item.Legendary == false)
                    {
                        item.SellIn = item.SellIn - 1;
                    }

                    // Quality is never greater than 50 unless Legendary
                    if ((item.Legendary == false) && item.Quality > 50)
                    {
                        item.Quality = 50;
                    }

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                }

                // log Daily run
                tbl_DailyUpdateLog slogEntry = new tbl_DailyUpdateLog();
                slogEntry.DateTime = DateTime.Now;
                slogEntry.Guid = Guid.NewGuid();
                if (manualRun)
                {
                    slogEntry.Type = "Success Manual";
                }
                else
                {
                    slogEntry.Type = "Success Automated";
                }
                db.tbl_DailyUpdateLog.Add(slogEntry);
                db.SaveChanges();

                // success
                return true;
                }

                // log Daily run
                tbl_DailyUpdateLog flogEntry = new tbl_DailyUpdateLog();
                flogEntry.DateTime = DateTime.Now;
                flogEntry.Guid = Guid.NewGuid();
                if (manualRun)
                {
                    flogEntry.Type = "Failed Manual";
                }
                else
                {
                    flogEntry.Type = "Failed Automated";
                }
                db.tbl_DailyUpdateLog.Add(flogEntry);
                db.SaveChanges();

                // failed
                return false;
            }

    }
}