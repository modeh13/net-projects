/**
 * This Scripts are used to work with offline mode.
 * 
 */
function confirmStoreOfflineModal() {

  jQuery.when(standAloneOffline(offlineDetails)).done(function(currentFormKey) {

    _saveData(currentFormKey, false);

    return currentFormKey;
  }).done(function(currentFormKey) {

    var formKey = btoa(currentFormKey);

    var url = jQuery('#contextPath').val() + '#mint?' + formKey;

    window.location.href = url;

  });
}

/**
 * This function save the forms in the localStorage (offline mode)
 */
function standAloneOffline(arrayDetails) {
  if (thereIsError) {
    return;
  }

  var formKeys = localStorage.getItem('key_forms');
  var arrayAllKeys = [];
  var numberForms = 0;

  if (formKeys != null) {
    arrayAllKeys = formKeys.split(';');
    numberForms = arrayAllKeys.length;
  }

  var completeHtml = document.getElementById("divContentForm").innerHTML;
  var parentDivCloned = document.getElementById("divContentForm").cloneNode(
      false);

  var color = parentDivCloned.style.backgroundColor;
  if (color === null || color === "") {
    color = "#eeeeee";
  }

  var individualKeys;
  var keysToDelete = [];

  var currentFormKey = setEntityKeys.join("-");

  for (var i = 0; i < numberForms; i++) {
    individualKeys = arrayAllKeys[i].split("-");
    var individualSize = individualKeys.length;
    for (var j = 0; j < individualSize; j++) {
      if (containsIntoArray(setEntityKeys, individualKeys[j])) {
        keysToDelete.push(arrayAllKeys[i]);
        break;
      }
    }
  }

  var sizeToDelete = keysToDelete.length;
  for (var x = 0; x < sizeToDelete; x++) {
    clearLocalStorageData(keysToDelete[x]);
  }

  try {
    var compressHtml = mintCompress(completeHtml);

    localStorage.setItem(currentFormKey, compressHtml);
    localStorage.setItem('form_info_' + currentFormKey, arrayDetails);

    // store class variables
    localStorage.setItem('stateForm_' + currentFormKey, this.stateForm);
    localStorage.setItem('widthFormValue_' + currentFormKey, this.maxWidthForm);

    // refresh key_forms
    formKeys = localStorage.getItem('key_forms');
    numberForms = 0;
    if (formKeys != null) {
      arrayAllKeys = formKeys.split(';');
      numberForms = arrayAllKeys.length;
    }

    if (numberForms > 0) {
      formKeys += ";" + currentFormKey;
    } else {
      formKeys = "" + currentFormKey;
    }
    localStorage.setItem('dateFormat', this.dateFormat);
    localStorage.setItem('entity_background_' + currentFormKey, color);
    localStorage.setItem('entity_height_' + currentFormKey,
        parentDivCloned.style.height);
    localStorage.setItem('rotateForm_' + currentFormKey, this.rotateStandAlone);
    localStorage.setItem('removeSelectRIButton_' + currentFormKey, this.removeSelectRIButton);
    localStorage.setItem('entityKeys_' + currentFormKey, this.setEntityKeys);

    localStorage.setItem('key_forms', formKeys);
  } catch (e) {
    window.clearTimeout(finishTimeOut);

    alert("Is not possible to store the form because the cache is full.");

    var backBtn = document.getElementById('back');
    if (backBtn) {
      backBtn.click();
    }
    return;
  }

  var submitBtn = document.getElementById('back');
  if (submitBtn) {
    submitBtn.click();
  }

  return currentFormKey;
}

function clearLocalStorageComponentsData(entity) {
  var allKeys = localStorage.getItem('filledComponentKeys_' + entity);
  if (allKeys) {
    var arrayAllKeys = mintDecompress(allKeys).split(';');
    var sizeKeys = arrayAllKeys.length;
    // remove values from localStorage
    for (var indexKey = 0; indexKey < sizeKeys; indexKey++) {
      localStorage.removeItem(arrayAllKeys[indexKey]);
    }
  }
  // clean all value keys from localstorage
  localStorage.removeItem('filledComponentKeys_' + entity);
  // clear invisible data
  localStorage.removeItem('invisible_' + entity);
}

/**
 * This function clean the LocalStorage to specific form references.
 * 
 * @param strEntityKeys
 */
function clearLocalStorageData(strEntityKeys) {
  if (strEntityKeys == null) {
    return;
  }

  var arrayFormKeys = [];
  var formsToRemove = [];
  var formIndex = 0;
  var entity;

  var formKeys = localStorage.getItem('key_forms');
  var numberForms = 0;
  if (formKeys != null) {
    arrayFormKeys = formKeys.split(';');
    numberForms = arrayFormKeys.length;
  }

  var entityKeys = strEntityKeys.split(",");
  var numberEntities = entityKeys.length;
  for (var index = 0; index < numberEntities; index++) {
    entity = entityKeys[index];

    clearLocalStorageComponentsData(entity);

    // remove utility objects from local storage
    localStorage.removeItem("styleComponents_" + entity);
    localStorage.removeItem('form_info_' + entity);
    localStorage.removeItem('widthFormValue_' + entity);
    localStorage.removeItem('stateForm_' + entity);
    localStorage.removeItem('entity_background_' + entity);
    localStorage.removeItem('entity_height_' + entity);
    localStorage.removeItem('rotateForm_' + entity);
    localStorage.removeItem('removeSelectRIButton_' + entity);
    localStorage.removeItem('entityKeys_' + entity);

    clearArraysLocalStorage('jsonComponentsBehavior', entity);
    clearArraysLocalStorage('jsonMandatories', entity);
    clearArraysLocalStorage('jsonMandatoryNames', entity);
    clearArraysLocalStorage('jsonHtmlEmployee', entity);
    clearArraysLocalStorage('jsonItemsBeforeChangeY', entity);

    // remove html by entity
    localStorage.removeItem(entity);

    // add entity to remove from cache
    formsToRemove[formIndex] = entity;
    formIndex++;
  }

  // update available entities in cache
  formKeys = "";
  var flagRemove = 0;
  for (var index2 = 0; index2 < numberForms; index2++) {
    if (formsToRemove.indexOf(arrayFormKeys[index2], 0) == -1) {
      if (flagRemove == 1) {
        formKeys += ";";
      }
      formKeys += arrayFormKeys[index2];
      flagRemove = 1;
    }
  }

  if (formKeys != "") {
    localStorage.setItem('key_forms', formKeys);
  } else {
    localStorage.removeItem('key_forms');
  }

  console.log('clearLocalStorageData executed...')
}

function clearArraysLocalStorage(arrayKey, entity) {
  var lenghtArrays = localStorage.getItem(arrayKey + '_' + entity + "_length");
  if (lenghtArrays != null && lenghtArrays > 0) {
    for (var index = 0; index < lenghtArrays; index++) {
      localStorage.removeItem(arrayKey + '_' + entity + '_' + index);
    }
  }
  localStorage.removeItem(arrayKey + '_' + entity + "_length");
}
