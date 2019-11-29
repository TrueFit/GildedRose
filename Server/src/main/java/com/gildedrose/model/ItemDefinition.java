package com.gildedrose.model;

import java.util.ArrayList;
import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToMany;
import javax.persistence.Table;

/**
 * Defines a specific type of item. It only provides the definition of an item.
 * Instances of items are represented by the {@code Item} entity.
 */
@Entity
@Table(name = "ITEM_DEFINITIONS")
public class ItemDefinition {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private long id;

	@Column(nullable = false, length = 100)
	private String name;

	@Column(nullable = true)
	private Boolean ignoreSellIn;

	@Column(nullable = true)
	private String qualityChangeExpression;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "CATEGORY_ID", nullable = false)
	private ItemCategory category;

	@OneToMany(mappedBy = "definition", cascade = CascadeType.PERSIST)
	private List<Item> items = new ArrayList<>();

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

	public ItemCategory getCategory() {
		return category;
	}

	public void setCategory(ItemCategory category) {
		this.category = category;
	}

	public List<Item> getItems() {
		return items;
	}

	public void setItems(List<Item> items) {
		this.items = items;
	}

	/**
	 * Indicates whether sell-in is ignored for this item by first looking for an
	 * explicit answer on the item definition, and then on the category. Otherwise
	 * false is returned.
	 */
	public boolean computeIgnoreSellIn() {
		if (getIgnoreSellIn() != null)
			return getIgnoreSellIn();

		if (category.getIgnoreSellIn() != null)
			return category.getIgnoreSellIn();

		return false;
	}

	/**
	 * Returns any custom quality change expression by first looking for it at the
	 * item definition, and then on the category. Otherwise null is returned.
	 */
	public String computeQualityChangeExpression() {
		if (getQualityChangeExpression() != null)
			return getQualityChangeExpression();

		if (category.getQualityChangeExpression() != null)
			return category.getQualityChangeExpression();

		return null;
	}
}
