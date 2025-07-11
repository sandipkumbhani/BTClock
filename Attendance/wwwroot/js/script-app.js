(function ($) {
  'use strict';

  function updateSidenavSize() {
    const width = window.innerWidth;
    const body = document.body;

    if (width <= 767) {
      body.setAttribute("data-sidenav-size", "full");         // Mobile
    } else if (width > 767 && width <= 1024) {
      body.setAttribute("data-sidenav-size", "condensed");    // Tablet
    } else {
      body.setAttribute("data-sidenav-size", "default");      // Desktop
    }
  }

  // Run on initial load
  updateSidenavSize();

  // Update on resize
  window.addEventListener("resize", updateSidenavSize);

  $(document).ready(function() {
      // Open modal via <a> tag
      $('.delete-modal').click(function(e) {
          e.preventDefault(); // Prevent the link from navigating
          $('.customModal').fadeIn();
      });

      // Close modal by close button
      $('.close-btn, .btn-tohide').click(function() {
          $('.customModal').fadeOut();
      });

      // Close modal by clicking overlay
      $('.modal-overlay').click(function() {
          $('.customModal').fadeOut();
      });
  });

 // sidebar submenu collapsible js
  // $(".side-nav .side-nav-item").on("click", function(){
  //   var item = $(this);
  //   item.siblings(".side-nav-item").children(".sidebar-submenu").slideUp();

  //   item.siblings(".side-nav-item").removeClass("dropdown-open");

  //   item.siblings(".side-nav-item").removeClass("open");

  //   item.children(".sidebar-submenu").slideToggle();

  //   item.toggleClass("dropdown-open");
  // });

  // Sidebar submenu collapsible JS
$(".side-nav .side-nav-item > a").on("click", function (e) {
    var item = $(this).parent();

    // Close other open submenus
    item.siblings(".side-nav-item").removeClass("dropdown-open")
        .children(".sidebar-submenu").slideUp();

    // Toggle current submenu
    item.toggleClass("dropdown-open")
        .children(".sidebar-submenu").slideToggle();

    e.stopPropagation(); // Prevent bubbling
});

// Sub-childmenu collapsible
$(".sidebar-submenu > li > a").on("click", function (e) {
    var subItem = $(this).next(".nav-sub-childmenu");
    if (subItem.length) {
        e.preventDefault(); // Prevent link from navigating
        e.stopPropagation(); // Prevent parent click handler

        // Close sibling sub-childmenus
        $(this).parent().siblings("li").removeClass("child-open")
            .find(".nav-sub-childmenu").slideUp();

        // Toggle current sub-childmenu
        $(this).parent().toggleClass("child-open");
        subItem.slideToggle();
    }
});


 
  $(".sidebar-toggle").on("click", function(){
    $(this).toggleClass("active");
    $(".leftside-menu").toggleClass("active");
    $(".content-page, .navbar-custom").toggleClass("active");
  });

  $(".button-toggle-menu").on("click", function(){
    $(".leftside-menu").addClass("sidebar-open");
    $("body").addClass("overlay-active");
  });

  $(".sidebar-close-btn").on("click", function(){
    $(".leftside-menu").removeClass("sidebar-open");
    $("body").removeClass("overlay-active");
  });

  //to keep the current page active
  $(function () {
    for (
      var nk = window.location,
        o = $("ul#leftside-menu-container a")
          .filter(function () {
            return this.href == nk;
          })
          .addClass("active-page") // anchor
          .parent()
          .addClass("active-page");
      ;

    ) {
      // li
      if (!o.is("li")) break;
      o = o.parent().addClass("show").parent().addClass("open");
    }
  });


$(document).ready(function() {
  $('.select2-custom').select2({
    theme: 'bootstrap4',
    placeholder: function () {
      return $(this).data('placeholder'); // dynamic placeholder
    },
    allowClear: true
  });
});

// Listen for click on toggle checkbox
$('#select-all').click(function(event) {   
    if(this.checked) {
        // Iterate each checkbox
        $(':checkbox').each(function() {
            this.checked = true;                        
        });
    } else {
        $(':checkbox').each(function() {
            this.checked = false;                       
        });
    }
});

Dropzone.autoDiscover = false;

$(document).ready(function () {
  let myDropzone = new Dropzone("#my-dropzone", {
    url: "/upload", // your server upload endpoint
    addRemoveLinks: false, // disable default remove button
    init: function () {
      this.on("addedfile", function (file) {
        // Add custom close button
        var removeButton = Dropzone.createElement("<div class='dz-remove-btn'>&times;</div>");

        // Append it to the file preview element
        file.previewElement.appendChild(removeButton);

        // Handle remove event
        $(removeButton).on("click", function (e) {
          e.preventDefault();
          e.stopPropagation();

          // Remove file from dropzone
          myDropzone.removeFile(file);
          
          // Optionally: Send request to delete from server if already uploaded
          // $.post("/delete", { filename: file.name });
        });
      });
    }
  });
});


  $(".show-hide").show();
  $(".show-hide span").addClass("show");

  $(".show-hide span").click(function () {
    if ($(this).hasClass("show")) {
      $('input[name="login[password]"]').attr("type", "text");
      $(this).removeClass("show");
    } else {
      $('input[name="login[password]"]').attr("type", "password");
      $(this).addClass("show");
    }
  });
  $('form button[type="submit"]').on("click", function () {
    $(".show-hide span").addClass("show");
    $(".show-hide")
      .parent()
      .find('input[name="login[password]"]')
      .attr("type", "password");
  });

const forms = document.querySelectorAll('.requires-validation')
Array.from(forms)
  .forEach(function (form) {
    form.addEventListener('submit', function (event) {
      if (!form.checkValidity()) {
        event.preventDefault()
        event.stopPropagation()
      }

      form.classList.add('was-validated')
    }, false)
  })


// /**
// * Utility function to calculate the current theme setting based on localStorage.
// */
// function calculateSettingAsThemeString({ localStorageTheme }) {
//   if (localStorageTheme !== null) {
//     return localStorageTheme;
//   }
//   return "light"; // default to light theme if nothing is stored
// }

// /**
// * Utility function to update the button text and aria-label.
// */
// function updateButton({ buttonEl, isDark }) {
//   const newCta = isDark ? "dark" : "light";
//   buttonEl.setAttribute("aria-label", newCta);
//   buttonEl.innerText = newCta;
// }

// /**
// * Utility function to update the theme setting on the html tag.
// */
// function updateThemeOnHtmlEl({ theme }) {
//   document.querySelector("html").setAttribute("data-theme", theme);
// }

// /**
// * 1. Grab what we need from the DOM and system settings on page load.
// */
// const button = document.querySelector("[data-theme-toggle]");
// const localStorageTheme = localStorage.getItem("theme");

// /**
// * 2. Work out the current site settings.
// */
// let currentThemeSetting = calculateSettingAsThemeString({ localStorageTheme });

// /**
// * 3. If the button exists, update the theme setting and button text according to current settings.
// */
// if (button) {
//   updateButton({ buttonEl: button, isDark: currentThemeSetting === "dark" });
//   updateThemeOnHtmlEl({ theme: currentThemeSetting });

//   /**
//   * 4. Add an event listener to toggle the theme.
//   */
//   button.addEventListener("click", (event) => {
//     const newTheme = currentThemeSetting === "dark" ? "light" : "dark";

//     localStorage.setItem("theme", newTheme);
//     updateButton({ buttonEl: button, isDark: newTheme === "dark" });
//     updateThemeOnHtmlEl({ theme: newTheme });

//     currentThemeSetting = newTheme;
//   });
// } else {
//   // If no button is found, just apply the current theme to the page
//   updateThemeOnHtmlEl({ theme: currentThemeSetting });
// }

})(jQuery);




$(document).ready(function () {
    // Get user's country via geo IP once
    $.get("https://ipapi.co/json/", function (data) {
      const userCountry = data.country_code || "us";

      // Initialize intl-tel-input for each phone input
      $(".phone-input").each(function () {
        window.intlTelInput(this, {
          initialCountry: userCountry.toLowerCase(),
          utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@25.3.1/build/js/utils.js"
        });
      });

    }).fail(function () {
      // If geo IP fails, default to US
      $(".phone-input").each(function () {
        window.intlTelInput(this, {
          initialCountry: "us",
          utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@25.3.1/build/js/utils.js"
        });
      });
    });
  });


// Function to initialize SimpleBar on elements with a given class
function initializeSimpleBarByClass(className) {
    const elements = document.querySelectorAll('.' + className);
    elements.forEach(el => {
    // Prevent duplicate initialization
    if (!el.getAttribute('data-simplebar')) {
        el.setAttribute('data-simplebar', '');
        new SimpleBar(el);
    }
    });
}

// Call function to apply SimpleBar to elements with class 'custom-scroll'
initializeSimpleBarByClass('custom-scroll');




$('#kt_docs_repeater_basic').repeater({
    initEmpty: false,

    defaultValues: {
        'text-input': 'foo'
    },

    show: function () {
        $(this).slideDown();

        // Re-init select2
        $(this).find('.select2-custom').select2({
          theme: 'bootstrap4',
          placeholder: function () {
            return $(this).data('placeholder'); // dynamic placeholder
          },
          allowClear: true
        });

    },

    hide: function (deleteElement) {
        $(this).slideUp(deleteElement);
    }
});


