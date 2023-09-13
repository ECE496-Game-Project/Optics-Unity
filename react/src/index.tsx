import { render } from '@reactunity/renderer';
import Panels from "./param_ui";
import './index.scss';

function App() {
  return<div>
          <Panels/>
        </div>;
}

render(<App />);
