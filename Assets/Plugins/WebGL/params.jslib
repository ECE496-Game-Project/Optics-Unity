var plugin = {
   ReceiveParams: function(floatData, idx){
         const floatValue = parseFloat(floatData);
         console.log('Receive Params ' + idx + ' From Unity ' + floatValue);
         if(idx === 0) {
            document.getElementById("eox").value = floatValue;
            params.Eox = floatValue;
         }
         if(idx === 1){
            document.getElementById("eoy").value = floatValue;
            params.Eoy = floatValue;
         }
         if(idx === 2) {
            document.getElementById("w").value = floatValue;
            params.W = floatValue;
         }
         if(idx === 3) {
            document.getElementById("k").value = floatValue;
            params.K = floatValue;
         }
         if(idx === 4) {
            document.getElementById("n").value = floatValue;
            params.N = floatValue;
         }
         if(idx === 5) {
            document.getElementById("theta").value = floatValue;
            params.Theta = floatValue;
         }
         if(idx === 6) {
            document.getElementById("phi").value = floatValue;
            params.Phi = floatValue;
         }
    },
}

mergeInto(LibraryManager.library, plugin);