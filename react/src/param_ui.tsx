import {useState} from 'react';
import './param_ui.scss'

export default function Panels() {
  return (
    <div className={"panels-container right-rectangle"}>
      <WaveSourcePanel/>
    </div>
  )
}

export function WaveSourcePanel() {
  const types = {
    INVALID: 0, PARALLEL: 1, POINT: 2,
  }

  return (
    <div className={"panel-container"}>
      <DropdownList name={'Type'} options={types}/>
      <InputField name={'Eox'}/>
      <InputField name={'Eoy'}/>
      <InputField name={'W'}/>
      <InputField name={'K'}/>
      <InputField name={'N'}/>
      <Slider name={'Theta'} min={'0'} max={'360'}/>
      <Slider name={'Phi'} min={'0'} max={'360'}/>
      <InputField name={'Distance'}/>
    </div>
  )
}

export function InputField({name}) {
  const [inputValue, setInputValue] = useState(1);

  function handleInputChange(e) {
    const value = e.target.value;
    setInputValue(value);
    //sendMessage("WaveLine", "Set" + name + "FromWeb", value);
  }

  return (
    <>
      <label className="param-label" htmlFor={name}>{name + ':'}</label>
      <input className="param-input" type="number" id={name} value={inputValue} onChange={handleInputChange}/>
    </>
  )
}

export function Slider({name, min, max}) {
  const [inputValue, setInputValue] = useState(180);

  function handleInputChange(e) {
    const value = e.target.value;
    setInputValue(value);
    //sendMessage("WaveLine", "Set" + name + "FromWeb", value);
  }

  return (
    <>
      <label className={"param-label"} htmlFor={name}>
        {name + ":"}
        <output>{inputValue}</output>
      </label>
      <input className={"param-input"} type="range" id={name} min={min} max={max}
             value={inputValue} onChange={handleInputChange}
      />
    </>
  );
}

export function DropdownList({name, options}) {
  function handleInputChange(e) {
    // const value = e.target.value;
    //sendMessage("WaveLine", "Set" + name + "FromWeb", value);
  }

  return (
    <>
      <label className={"param-label"} htmlFor={name}>{name + ':'}</label>
      <select className={"param-select"} id={name} onChange={handleInputChange}>
        {/*{*/}
        {/*  Object.values(options).map((value) => (*/}
        {/*    <option key={value} value={value}>*/}
        {/*      {value}*/}
        {/*    </option>*/}
        {/*  ))*/}
        {/*}*/}
      </select>
    </>
  )
}
