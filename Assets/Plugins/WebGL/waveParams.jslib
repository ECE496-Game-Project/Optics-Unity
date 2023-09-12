var plugin = {
   ReceiveParams: function(floatData, idx){
         const ParamDict = {
             0: "Eox",
             1: "Eoy",
             2: "W",
             3: "K",
             4: "N",
             5: "Theta",
             6: "Phi",
             7: "Distance",
         };

         const floatValue = parseFloat(floatData);
         document.getElementById(ParamDict[idx]).value = floatValue;
    },
   ReceiveWaveType: function(waveType){
        const selectElement = document.getElementById('Type');
        selectElement.selectedIndex = waveType;
   },
}

mergeInto(LibraryManager.library, plugin);