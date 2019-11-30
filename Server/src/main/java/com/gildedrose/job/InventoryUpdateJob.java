package com.gildedrose.job;

import java.time.LocalDate;

import org.quartz.Job;
import org.quartz.JobExecutionContext;
import org.quartz.JobExecutionException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.gildedrose.service.InventoryCalculationService;
import com.gildedrose.service.InventoryService;

/**
 * Brings the inventory calculations current with today. Ensures that even if
 * the job has not run in days, the calculations will be made for each day up
 * until today.
 */
@Component
public class InventoryUpdateJob implements Job {

	private static final Logger Log = LoggerFactory.getLogger(InventoryUpdateJob.class);

	@Autowired
	private InventoryService inventoryService;

	@Autowired
	private InventoryCalculationService inventoryCalculationService;

	/* -- PUBLIC METHODS -- */

	public void execute(JobExecutionContext context) throws JobExecutionException {
		Log.info("Starting inventory update job");

		// Ensure that the inventory date is caught up to the current day
		LocalDate inventoryDate = inventoryService.getInventoryDate().getDate();
		int days = 0;

		while (inventoryDate.compareTo(LocalDate.now()) < 0) {
			inventoryCalculationService.progressDate();
			inventoryDate = inventoryService.getInventoryDate().getDate();
			days++;

			Log.info(String.format("Progressed inventory to %s", inventoryDate));
		}

		Log.info(String.format("Finished inventory update job. Progressed %d days", days));
	}
}
