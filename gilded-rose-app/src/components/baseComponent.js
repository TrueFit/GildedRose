import React from 'react';

class BaseComponent extends React.PureComponent {
  _bind(...fn) {
    fn.forEach((method) => this[method] = this[method].bind(this));
  }
}

export default BaseComponent;