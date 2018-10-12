jQuery.fn.extend({
   setChosen: function (selector) {      
      $(selector).chosen({
         allow_single_deselect: true,
         autoclose: true,
         width: '100%'
      });
   },
   setDataTable: function (opts) {      
      var settings = {         
         order: [],
         ordering: false,
         destroy: true,
         responsive: true
      };

      settings = $.extend(settings, opts);
      $(this).DataTable(settings);
   }
});

function validateIntegerKeyPress(event) {
   return event.charCode >= 48 && event.charCode <= 57;
}

//Thanks: http://jsfiddle.net/S9G8C/203/
function validateFloatKeyPress(el, evt) {
   var charCode = (evt.which) ? evt.which : event.keyCode;
   var number = el.value.split('.');
   if (charCode !== 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
   }
   //just one dot
   if (number.length > 1 && charCode === 46) {
      return false;
   }
   //get the carat position
   var caratPos = getSelectionStart(el);
   var dotPos = el.value.indexOf(".");
   if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
      return false;
   }
   return true;
}

function getSelectionStart(o) {
   if (o.createTextRange) {
      var r = document.selection.createRange().duplicate();
      r.moveEnd('character', o.value.length);
      if (r.text === '') return o.value.length;
      return o.value.lastIndexOf(r.text);
   } else return o.selectionStart;
}

/// Show Notification
//Type: "success", "error", "info", "warning"
function Notification(title, message, type) {
   PNotify.prototype.options.styling = "fontawesome";

   new PNotify({
      title: title,
      text: message,
      type: type,
      history: {
         history: false
      },
      width: "60%",
      hide: true,
      delay: 2000,
      cornerclass: "",
      addclass: "stack-bar-bottom",
      buttons: {
         closer: true,
         sticker: true
      }
   });
}

// -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --
//Products
// -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --
function SetProductsTable(response) {
   var settings = {
      order: [],
      ordering: false,
      destroy: true,
      responsive: true,
      searching: false,
      processing: true,
      columns: [
         {
            width: "10%"
         },
         {
            width: "40%"
         },
         {
            width: "10%"
         },
         {
            width: "10%"
         },
         {
            width: "10%"
         }
      ]
   };

   if (response !== null) $("#dv-tbl-products").html(response);
   $("#tbl-products").setDataTable(settings);
}