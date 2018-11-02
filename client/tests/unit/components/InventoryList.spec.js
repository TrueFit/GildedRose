import {shallowMount} from '@vue/test-utils';
import InventoryList from '@/components/InventoryList.vue';

describe('inventory list', () => {
  let item;

  beforeEach(() => {
    item = {
      name: 'item',
      category: 'category',
      sellIn: 10,
      quality: 10,
    };
  });

  it('renders a table', () => {
    const items = [item];
    const wrapper = shallowMount(InventoryList, {
      propsData: { items },
      stubs: ['router-link'],
    });

    expect(wrapper.find('.inventory-list__empty').exists()).toBe(false);

    const listItems = wrapper.findAll('.inventory-list__table td');
    expect(listItems).toHaveLength(4);
    expect(listItems.at(0).text()).toEqual(item.name);
    expect(listItems.at(1).text()).toEqual(item.category);
    expect(listItems.at(2).text()).toEqual(item.sellIn.toString());
    expect(listItems.at(3).text()).toEqual(item.quality.toString());
  });

  it('renders a list', () => {
    const items = [item];
    const wrapper = shallowMount(InventoryList, {
      propsData: { items },
      stubs: ['router-link'],
    });

    expect(wrapper.find('.inventory-list__empty').exists()).toBe(false);
    const listItems = wrapper.findAll('.inventory-list__item');
    expect(listItems).toHaveLength(1);
  });

  it('renders an empty message', () => {
    const items = [];
    const wrapper = shallowMount(InventoryList, {
      propsData: { items },
      stubs: ['router-link'],
    });

    expect(wrapper.find('.inventory-list__table').exists()).toBe(false);

    const message = wrapper.find('.inventory-list__empty');
    expect(message.exists()).toBe(true);
    expect(message.text()).toEqual('No items!');
  });
});
