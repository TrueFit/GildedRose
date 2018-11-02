import {mount, shallowMount} from '@vue/test-utils';
import flushPromises from 'flush-promises';
import Inventory from '@/views/Inventory.vue';
import {advance, find} from '@/services/Inventory';
jest.mock('@/services/Inventory');

describe('inventory', () => {
  const message = wrapper => wrapper.find('.inventory__message');
  const errorMessage = wrapper => wrapper.find('.inventory__error');
  const content = wrapper => wrapper.find('.inventory__content');
  const button = wrapper => wrapper.find('button');

  beforeEach(() => {
    advance.mockReset();
    find.mockReset();
  });

  it('shows the content when there are no messages', async () => {
    find.mockReturnValue(Promise.resolve([]));
    const wrapper = shallowMount(Inventory);
    expect(find).lastCalledWith({});
    await flushPromises();

    expect(errorMessage(wrapper).exists()).toBe(false);
    expect(message(wrapper).exists()).toBe(false);
    expect(content(wrapper).exists()).toBe(true);
  });

  describe('fetching', () => {
    it('shows a message', done => {
      find.mockReturnValue(Promise.resolve([]));
      const wrapper = shallowMount(Inventory);
      expect(find).lastCalledWith({});

      // using next tick so that we can check things before the promise finishes
      wrapper.vm.$nextTick(() => {
        expect(errorMessage(wrapper).exists()).toBe(false);
        expect(content(wrapper).exists()).toBe(false);
        const messageWrapper = message(wrapper);
        expect(messageWrapper.exists()).toBe(true);
        expect(messageWrapper.text()).not.toEqual('');
        expect(wrapper.vm.message).not.toBeNull();
        done();
      });
    });

    it('shows an error message', async () => {
      find.mockReturnValue(Promise.reject([]));
      const wrapper = shallowMount(Inventory);
      expect(find).lastCalledWith({});
      await flushPromises();

      expect(button(wrapper).attributes('disabled')).toEqual('disabled');
      expect(content(wrapper).exists()).toBe(false);
      expect(message(wrapper).exists()).toBe(false);
      const errorWrapper = errorMessage(wrapper);
      expect(errorWrapper.exists()).toBe(true);
      expect(errorWrapper.text()).not.toEqual('');
      expect(wrapper.vm.error).not.toBeNull();
    });
  });

  describe('filter', () => {
    it('updates the filter when you change the field', () => {
      find.mockReturnValue(Promise.resolve([]));
      const wrapper = shallowMount(Inventory);

      const field = wrapper.findAll('option').at(1);
      field.trigger('change');
      expect(find).lastCalledWith({quality: 0});
    });

    it('clears the filter', () => {
      find.mockReturnValue(Promise.resolve([]));
      const wrapper = shallowMount(Inventory);

      const field = wrapper.findAll('option').at(0);
      field.trigger('change');
      expect(find).lastCalledWith({});
    });
  });

  describe('advance to next day', () => {
    it('shows a message', async () => {
      find.mockReturnValue(Promise.resolve([]));
      advance.mockReturnValue(Promise.resolve([]));
      const wrapper = shallowMount(Inventory);

      const button = wrapper.find('button');
      button.trigger('click');

      expect(advance).toBeCalled();

      expect(errorMessage(wrapper).exists()).toBe(false);
      expect(content(wrapper).exists()).toBe(false);
      const messageWrapper = message(wrapper);
      expect(messageWrapper.exists()).toBe(true);
      expect(messageWrapper.text()).not.toEqual('');
      expect(wrapper.vm.message).not.toBeNull();

      await flushPromises();
      expect(find).lastCalledWith({});
    });

    it('shows an error message', async () => {
      find.mockReturnValue(Promise.resolve([]));
      advance.mockReturnValue(Promise.reject([]));
      const wrapper = shallowMount(Inventory);

      const buttonWrapper = button(wrapper);
      buttonWrapper.trigger('click');

      await flushPromises();

      expect(advance).toBeCalled();
      expect(find).toHaveBeenCalledTimes(1);

      expect(buttonWrapper.attributes('disabled')).toEqual('disabled');
      expect(message(wrapper).exists()).toBe(false);
      expect(content(wrapper).exists()).toBe(false);
      const errorWrapper = errorMessage(wrapper);
      expect(errorWrapper.exists()).toBe(true);
      expect(errorWrapper.text()).not.toEqual('');
      expect(wrapper.vm.error).not.toBeNull();
    });
  });
});
