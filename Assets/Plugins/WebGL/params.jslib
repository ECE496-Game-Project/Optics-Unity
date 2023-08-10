var plugin = {
   ReceiveParams: function(floatData, idx){
         const floatValue = parseFloat(floatData);
         document.getElementById(ParamDict[idx]).value = floatValue;
    },
   ReceiveWaveType: function(waveType){
        const selectElement = document.getElementById('type');
        selectElement.selectedIndex = waveType;
   },
}

mergeInto(LibraryManager.library, plugin);