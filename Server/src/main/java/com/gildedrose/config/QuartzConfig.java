package com.gildedrose.config;

import javax.annotation.PostConstruct;

import org.quartz.JobDetail;
import org.quartz.SimpleTrigger;
import org.quartz.Trigger;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.condition.ConditionalOnExpression;
import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.ClassPathResource;
import org.springframework.scheduling.quartz.JobDetailFactoryBean;
import org.springframework.scheduling.quartz.SchedulerFactoryBean;
import org.springframework.scheduling.quartz.SimpleTriggerFactoryBean;
import org.springframework.scheduling.quartz.SpringBeanJobFactory;

import com.gildedrose.job.InventoryUpdateJob;

@Configuration
@ConditionalOnExpression("'${app.quartz.enable}'=='true'")
public class QuartzConfig {

	private static final Logger Log = LoggerFactory.getLogger(QuartzConfig.class);

	@Value("${app.quartz.interval}")
	private int jobInterval;

	@Autowired
	private ApplicationContext applicationContext;

	@PostConstruct
	public void init() {
		Log.info("Configuring Quartz");
	}

	@Bean
	public SpringBeanJobFactory springBeanJobFactory() {
		Log.debug("Configuring job factory");

		AutowiringSpringBeanJobFactory jobFactory = new AutowiringSpringBeanJobFactory();
		jobFactory.setApplicationContext(applicationContext);

		return jobFactory;
	}

	@Bean
	public SchedulerFactoryBean scheduler(Trigger trigger, JobDetail job) {
		Log.debug("Configuring job scheduler");

		SchedulerFactoryBean schedulerFactory = new SchedulerFactoryBean();
		schedulerFactory.setConfigLocation(new ClassPathResource("quartz.properties"));
		schedulerFactory.setJobFactory(springBeanJobFactory());
		schedulerFactory.setJobDetails(job);
		schedulerFactory.setTriggers(trigger);

		return schedulerFactory;
	}

	@Bean
	public JobDetailFactoryBean jobDetail() {

		JobDetailFactoryBean jobDetailFactory = new JobDetailFactoryBean();
		jobDetailFactory.setJobClass(InventoryUpdateJob.class);
		jobDetailFactory.setName("Qrtz_Job_Detail");
		jobDetailFactory.setDescription("Invoke service to update inventory");
		jobDetailFactory.setDurability(true);
		return jobDetailFactory;
	}

	@Bean
	public SimpleTriggerFactoryBean trigger(JobDetail job) {

		SimpleTriggerFactoryBean trigger = new SimpleTriggerFactoryBean();
		trigger.setJobDetail(job);

		Log.info("Configuring trigger to fire every {} seconds", jobInterval);

		trigger.setStartDelay(10000);
		trigger.setRepeatInterval(jobInterval * 1000);
		trigger.setRepeatCount(SimpleTrigger.REPEAT_INDEFINITELY);
		trigger.setName("Qrtz_Trigger");
		return trigger;
	}
}