package com.gildedrose.model;

import java.util.ArrayList;
import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.OneToMany;
import javax.persistence.Table;

/**
 * Defines a category to which items are assigned, via their definition. An item
 * is associated to one (and only one) category.
 */
@Entity
@Table(name = "ITEM_CATEGORIES")
public class ItemCategory {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private long id;

	@Column(nullable = false, length = 100)
	private String name;

	@Column(nullable = true)
	private Boolean ignoreSellIn;

	@Column(nullable = true)
	private String qualityChangeExpression;

	@OneToMany(mappedBy = "category", cascade = CascadeType.PERSIST)
	private List<ItemDefinition> definitions = new ArrayList<>();

	/* -- PUBLIC METHODS -- */

	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Boolean getIgnoreSellIn() {
		return ignoreSellIn;
	}

	public void setIgnoreSellIn(Boolean ignoreSellIn) {
		this.ignoreSellIn = ignoreSellIn;
	}

	public String getQualityChangeExpression() {
		return qualityChangeExpression;
	}

	public void setQualityChangeExpression(String qualityChangeExpression) {
		this.qualityChangeExpression = qualityChangeExpression;
	}

	public List<ItemDefinition> getDefinitions() {
		return definitions;
	}

	public void setDefinitions(List<ItemDefinition> definitions) {
		this.definitions = definitions;
	}
}
