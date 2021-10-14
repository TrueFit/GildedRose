package com.moose.gildedrose.security;

import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;

/**
 * Provides Spring {@link org.springframework.context.annotation.Bean}s related to application security.
 */
@Configuration
@EnableWebSecurity
public class WebSecurityConfig extends WebSecurityConfigurerAdapter {

	/**
	 * Configure the web tier security to remove cors restrictions.
	 * This allows for same-site traffic.
	 * @param http {@link HttpSecurity} configuration to modify.
	 * @throws Exception when invalid configuration occurs.
	 */
	@Override
	protected void configure(final HttpSecurity http) throws Exception {
		http.cors().and().csrf().disable();
	}
}
