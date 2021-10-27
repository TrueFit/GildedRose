import './App.css';
import { BrowserRouter } from 'react-router-dom';

import NavigationRouter from './NavigationRouter';

function App() {
  return (
    <div className="container">
        <BrowserRouter>
          <NavigationRouter/>
        </BrowserRouter>
    </div>
  );
}

export default App;
