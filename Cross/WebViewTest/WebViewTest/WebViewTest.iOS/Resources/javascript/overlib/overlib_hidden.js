//AK, 12.10.2011, DEV-745
//Plugin for overlib to take content from some hidden element and show it in overlib popup.
//Instead of usual content string the hidden element ID should be given.


registerCommands('showhidden');

////////
// DEFAULT CONFIGURATION
// Settings you want everywhere are set here. All of this can also be
// changed on your html page or through an overLIB call.
////////
if (typeof ol_showhidden == 'undefined') var ol_showhidden = 0;


////////
// INIT
////////
// Runtime variables init. Don't change for config!
var o3_showhidden = 0;

////////
// PLUGIN FUNCTIONS
////////

function SHOWHIDDEN_setVariables() {
  o3_showhidden=ol_showhidden;
}

function SHOWHIDDEN_parseExtras(pf,i,ar) { 
  var k=i,v; 
  if (k < ar.length) { 
    if (ar[k]==SHOWHIDDEN) { eval(pf +'showhidden=('+pf+'showhidden==0) ? 1 : 0'); return k; } 
  }
  return -1; 
} 

function SHOWHIDDEN_replaceContentText(){
  if(o3_showhidden){
    var elem = document.getElementById(o3_text);
    if(elem){
      o3_text = elem.innerHTML;
    }else{
      o3_text = '<empty>';
    }
  }
}


////////
// PLUGIN REGISTRATIONS
////////
registerRunTimeFunction(SHOWHIDDEN_setVariables);
registerCmdLineFunction(SHOWHIDDEN_parseExtras);
registerHook("olMain",SHOWHIDDEN_replaceContentText,FBEFORE);
if (olInfo.meets(4.10)) registerNoParameterCommands('showhidden');
