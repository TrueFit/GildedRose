import {shallowMount} from '@vue/test-utils';
import flushPromises from 'flush-promises';
import InventoryList from '@/components/InventoryList.vue';
import {find} from '@/services/Inventory';
jest.mock('@/services/Inventory');

describe('inventory list', () => {
  let item;

  beforeEach(() => {
    find.mockReset();

    item = {
      name: 'item',
      category: 'category',
      sellIn: 10,
      quality: 10,
    };
  });

  it('renders a loading message', done => {
    find.mockReturnValue(Promise.resolve([]));
    const wrapper = shallowMount(InventoryList);
    expect(find).toBeCalledWith({});

    // using nextTick so this evaluates before the mocked promise returns
    wrapper.vm.$nextTick(() => {
      expect(wrapper.find('.inventory-list__error').exists()).toBe(false);
      expect(wrapper.find('.inventory-list__list').exists()).toBe(false);
      expect(wrapper.find('.inventory-list__empty').exists()).toBe(false);

      const message = wrapper.find('.inventory-list__loading');
      expect(message.exists()).toBe(true);
      expect(message.text()).toEqual('Fetching inventory...');
      expect(wrapper.vm.isFetching).toBe(true);
      done();
    });
  });

  it('renders an error message', async () => {
    const error = 'Helpful error message!';
    find.mockReturnValue(Promise.reject(error));
    const wrapper = shallowMount(InventoryList);
    expect(find).toBeCalledWith({});

    await flushPromises();

    expect(wrapper.find('.inventory-list__loading').exists()).toBe(false);
    expect(wrapper.find('.inventory-list__list').exists()).toBe(false);
    expect(wrapper.find('.inventory-list__empty').exists()).toBe(false);

    const message = wrapper.find('.inventory-list__error');
    expect(message.exists()).toBe(true);
    expect(message.text()).toEqual(error);
    expect(wrapper.vm.isFetching).toBe(false);
    expect(wrapper.vm.error).toBe(error);
    expect(wrapper.vm.inventory).toBe(null);
  });

  it('renders the table', async () => {
    const inventory = [item];
    find.mockReturnValue(Promise.resolve(inventory));
    const wrapper = shallowMount(InventoryList);
    expect(find).toBeCalledWith({});

    await flushPromises();

    expect(wrapper.find('.inventory-list__loading').exists()).toBe(false);
    expect(wrapper.find('.inventory-list__error').exists()).toBe(false);
    expect(wrapper.find('.inventory-list__empty').exists()).toBe(false);
    expect(wrapper.find('.inventory-list__list').exists()).toBe(true);

    const listItems = wrapper.findAll('.inventory-list__list td');
    expect(listItems).toHaveLength(4);
    expect(listItems.at(0).text()).toEqual(item.name);
    expect(listItems.at(1).text()).toEqual(item.category);
    expect(listItems.at(2).text()).toEqual(item.sellIn.toString());
    expect(listItems.at(3).text()).toEqual(item.quality.toString());

    expect(wrapper.vm.isFetching).toBe(false);
    expect(wrapper.vm.error).toBe(null);
    expect(wrapper.vm.inventory).toEqual(inventory);
  });

  it('renders an empty message', async () => {
    find.mockReturnValue(Promise.resolve([]));
    const wrapper = shallowMount(InventoryList);
    expect(find).toBeCalledWith({});

    await flushPromises();

    expect(wrapper.find('.inventory-list__loading').exists()).toBe(false);
    expect(wrapper.find('.inventory-list__error').exists()).toBe(false);
    expect(wrapper.find('.inventory-list__list').exists()).toBe(false);

    const message = wrapper.find('.inventory-list__empty');
    expect(message.exists()).toBe(true);
    expect(message.text()).toEqual('No items!');
    expect(wrapper.vm.isFetching).toBe(false);
    expect(wrapper.vm.error).toBe(null);
    expect(wrapper.vm.inventory).toEqual([]);
  });

  it('passes the filter along', async () => {
    const filter = {quality: 0};
    find.mockReturnValue(Promise.resolve([item]));
    const wrapper = shallowMount(InventoryList, {
      propsData: {filter},
    });
    expect(find).toBeCalledWith(filter);
  });

  it('refreshes when the filter changes', async () => {
    const filter = {quality: 0};
    find.mockReturnValue(Promise.resolve([item]));
    const wrapper = shallowMount(InventoryList);
    wrapper.setProps({filter});
    expect(find).toBeCalledWith(filter);
  });
});
