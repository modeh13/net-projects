
function changeComponentsValues(elemId1, value1, elemId2, value2, treeDiv) { 
  try {
    selectedValue = document.getElementById(elemId2).value;
    if (selectedValue) {
      divName  = 'div_name_' + selectedValue;
      divName2 = 'div_name_' + selectedValue + '_' + elemId2;
      if (divName) {
        divSelected = document.getElementById(divName);
        if (divSelected) {
          divSelected.style.background = '';
          divSelected.style.color = 'black';
        }
      }
      if (divName2) {
        divSelected2 = document.getElementById(divName2);
        if (divSelected2) {
          divSelected2.style.background = '';
          divSelected2.style.color = 'black';
        }
      }
    }
    
    newDivName = 'div_name_' + value2;
    newDivName2 = 'div_name_' + value2 + '_' + elemId2;
    
    if (newDivName) {
      newSelectedObj = document.getElementById(newDivName);
      if (newSelectedObj) {
        newSelectedObj.style.background = '#6698FF';
        newSelectedObj.style.color = 'white';
      }
    }
    
    if (newDivName2) {
      newSelectedObj2 = document.getElementById(newDivName2);
      if (newSelectedObj2) {
        newSelectedObj2.style.background = '#6698FF';
        newSelectedObj2.style.color = 'white';
      }
    }
    
    var comp;
    comp = document.getElementById(elemId1);
    if (comp) {
      comp.value = value1;
      if (comp.onchange) comp.onchange();
    }
    comp = document.getElementById(elemId2);
    if (comp) {
      comp.value = value2;
      if (comp.onchange) comp.onchange();
    }
    
    return true;
  }
  catch (error) {
    return false;
  }
} 


//---------------------------------------------------------------
// Validate all margin input fields and submit form if all are OK
//---------------------------------------------------------------
function validateAndClose(elemId1, elemId2) {
  elementOpenerName = window.opener.document.getElementById(elemId1);
  elementOpenerValue = window.opener.document.getElementById(elemId2);
  elementName = replaceHTMLTags(document.getElementById(elemId1).value);
  elementValue = document.getElementById(elemId2).value;
  
  if (elementOpenerName && elementOpenerValue) {
    elementOpenerName.value = elementName;
    elementOpenerValue.value = elementValue;
  }
  
  window.close();
}

function replaceHTMLTags(value){
  value = value.replace(/&amp;/g, "&");
  value = value.replace(/&auml;/g, "ä");
  value = value.replace(/&Auml;/g, "Ä");
  value = value.replace(/&ouml;/g, "ö");
  value = value.replace(/&Ouml;/g, "Ö");
  value = value.replace(/&uuml;/g, "ü");
  value = value.replace(/&Uuml;/g, "Ü");
  value = value.replace(/&szlig;/g, "ß");
  value = value.replace(/&lt;/g, "<");
  value = value.replace(/&gt;/g, ">");
  value = value.replace(/&apos;/g, "'");
  value = value.replace(/&quot;/g, "\"");
  value = value.replace(/<br>/g, "\r\n");
  value = value.replace(/<br>/g, "\r");
  value = value.replace(/<br>/g, "\n");
  return value;
}


//---------------------------------------------------------------------
// Open the page setup dlg for a specific report id and name the window
//---------------------------------------------------------------------
function popupPageTree(windowURL, windowName, w, h, elementId) {
  // initialize vars for displaying in center of the screen
  element = document.getElementById(elementId);
  if (element) {
    value = element.value;
    if (value) {
      windowURL = windowURL + '/' + value;
    }
    else {
      windowURL = windowURL + '/-1';
    }
  }
  var xpos = (screen.width - w) / 2;
  var ypos = (screen.height - h) / 2;
  return window.open(windowURL, // URL
    windowName,  // window name for later reference
    "scrollbars=no,status=no,toolbar=no,location=no,directories=no,resizable=no,menubar=no," +
    "width=" + w + ",height=" + h + ",screenX=" + xpos + ",screenY=" + ypos + ",top=" + ypos +
    ",left="+xpos); // additional window options
}
