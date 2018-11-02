import {mount, shallowMount} from '@vue/test-utils';
import flushPromises from 'flush-promises';
import ItemDetails from '@/views/ItemDetails.vue';
import {get} from '@/services/Inventory';
jest.mock('@/services/Inventory');

describe('inventory', () => {
  let item;

  const errorMessage = w => w.find('.item-details__error');
  const message = w => w.find('.item-details__message');
  const content = w => w.find('.item-details__content');

  beforeEach(() => {
    get.mockReset();
    item = {
      name: 'Aged Brie',
      category: 'Food',
      sellIn: 10,
      quality: 10,
    };
  });

  it('shows the item', async () => {
    get.mockReturnValue(Promise.resolve(item));
    const $route = {params: {name: item.name}};
    const wrapper = shallowMount(ItemDetails, {
      mocks: {$route},
      stubs: ['router-link'],
    });
    await flushPromises();

    expect(get).lastCalledWith(item.name);
    expect(wrapper.vm.item).toEqual(item);
    expect(errorMessage(wrapper).exists()).toBe(false);
    expect(message(wrapper).exists()).toBe(false);
    expect(content(wrapper).exists()).toBe(true);
  });

  it('shows a loading message', done => {
    get.mockReturnValue(Promise.resolve(item));
    const $route = {params: {name: item.name}};
    const wrapper = shallowMount(ItemDetails, {
      mocks: {$route},
      stubs: ['router-link'],
    });

    wrapper.vm.$nextTick(() => {
      expect(get).lastCalledWith(item.name);
      expect(errorMessage(wrapper).exists()).toBe(false);
      expect(message(wrapper).exists()).toBe(true);
      expect(content(wrapper).exists()).toBe(false);
      done();
    });
  });

  it('shows an error message', async () => {
    get.mockReturnValue(Promise.reject({message: 'error'}));
    const $route = {params: {name: item.name}};
    const wrapper = shallowMount(ItemDetails, {
      mocks: {$route},
      stubs: ['router-link'],
    });
    await flushPromises();

    expect(get).lastCalledWith(item.name);
    expect(errorMessage(wrapper).exists()).toBe(true);
    expect(message(wrapper).exists()).toBe(false);
    expect(content(wrapper).exists()).toBe(false);
  });
});
