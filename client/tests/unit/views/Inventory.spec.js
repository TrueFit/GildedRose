import {mount, shallowMount} from '@vue/test-utils';
import flushPromises from 'flush-promises';
import Inventory from '@/views/Inventory.vue';
import {advance} from '@/services/Inventory';
jest.mock('@/services/Inventory');

describe('inventory', () => {
  let wrapper;

  beforeEach(() => {
    wrapper = shallowMount(Inventory);
    advance.mockReset();
  });

  describe('filter', () => {
    it('updates the filter when you change the field', () => {
      const field = wrapper.findAll('option').at(1);
      field.trigger('change');
      expect(wrapper.vm.filter).toEqual({quality: 0});
    });

    it('clears the filter', () => {
      const field = wrapper.findAll('option').at(0);
      field.trigger('change');
      expect(wrapper.vm.filter).toEqual({});
    });
  });

  describe('advance to next day', () => {
    it('shows a message', () => {
      advance.mockReturnValue(Promise.resolve([]));

      const button = wrapper.find('button');
      button.trigger('click');

      const message = wrapper.find('.inventory__message');
      expect(message.exists()).toBe(true);
      expect(message.text()).toEqual('Updating inventory...');
      expect(wrapper.vm.message).toBe('Updating inventory...');
      expect(advance).toBeCalled();
    });

    it('shows an error message', async () => {
      advance.mockReturnValue(Promise.reject([]));

      const button = wrapper.find('button');
      button.trigger('click');

      await flushPromises();

      const text = 'There was a problem updating the inventory. Please refresh and try again.';
      const message = wrapper.find('.inventory__error');
      expect(message.exists()).toBe(true);
      expect(message.text()).toEqual(text);
      expect(wrapper.vm.error).toBe(text);
      expect(advance).toBeCalled();
    });
  });
});
