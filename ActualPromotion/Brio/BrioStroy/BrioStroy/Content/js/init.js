function getCompanyWorks(callBack) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: "/Articles/GetCompanyWorks",
            success: function (response) {
                callBack(response);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("request failed");
            },

            processData: false,
            async: false
        });
}

function showOrHidePageLogo(showingBlock) {
    var blocksCollection = $(".block_ins");
    if (showingBlock.hasClass("partner")) {
        if ($(window).height() < 700 || blocksCollection.index(showingBlock) == blocksCollection.length - 1) {
            $(".bottom_arrows_container").hide();
        }

        if ($(window).height() >= 800) {
            if (!showingBlock.hasClass("products")) {
                $(".page_logo").css("background-image", "url('../Content/images/partners.png')");
            }
        }
    }
    else {
        $(".bottom_arrows_container").show();
        if ($(window).height() >= 860) {
            if (!showingBlock.hasClass("products")) {
                $(".page_logo").css("background-image", "url('../Content/images/about_company.png')");
            }
        }
    }
}
var isBusy = false;
function removeBlock(duration, onComplete) {
    if (isBusy) {
        return false;
    }
    isBusy = true;
    var currentBlock = $(".block_ins.swiper-slide-active");

    currentBlock.toggle("drop", { direction: "right" }, duration, function () {
      currentBlock.remove();
      renderNavPanel();
      isBusy = false;
      onComplete();
      mySwiper.reInit();
    });
}
var mySwiper = {};
function renderNavPanel() {
  $(".units-nav-container").empty();
  for (var i = 0; i < $(".block_ins:visible").length; i++) {
    if ($(".unit_nav").length == 0) {
      $(".units-nav-container").append('<div class="u-nav-container"><div class="unit_nav"></div></div>');
    }
    else {
      $(".u-nav-container").last().after('<div class="u-nav-container"><div class="unit_nav"></div></div>');
    }

    var PARTNERS_WORD = 'оюпрмепш';
    if (i == $(".block_ins").length - 1) {
      $(".unit_nav").last().after('<div class="block-desc disp_none">' + PARTNERS_WORD + '<div class="arrow_corner"></div></div>');
    }
  }

  $(".u-nav-container").css({ "height": (100 / $(".unit_nav").length) - 2 + "%" });
  $(".unit_nav").eq(0).addClass("active");

  $(".unit_nav").bind("click", function () {
    mySwiper.swipeTo($(".unit_nav").index($(this)));
  });
}
(function ($) {

    var settings = {
        // Fullscreen?
        fullScreen: true,

        // Section Transitions?
        sectionTransitions: true,

        // Fade in speed (in ms).
        fadeInSpeed: 1000,
        minWidth: 990,
        minHeight: 480
    };

    $(function () {
        var $window = $(window),
            $body = $('body'),
            $header = $('#header'),
            $footer = $('#footer'),
            $all = $body.add($header).add($footer),
            sectionTransitionState = false;

        $("body").css("overflow", "hidden");

        var newWidth = $window.width();
        var newHeight = $window.height() - $header.outerHeight() - $footer.outerHeight();
        $(".content .slider").css('height', newHeight > settings.minHeight ? newHeight : settings.minHeight);
        $(".content .slider").css('width', newWidth > settings.minWidth ? newWidth : settings.minWidth);
        //$(".content .cont").css('width', ((newWidth > settings.minWidth ? newWidth : settings.minWidth) * 3));

        // Resize.
        var resizeTimeout, resizeScrollTimeout;

        $window.resize(function () {
            window.clearTimeout(resizeTimeout);
            resizeTimeout = window.setTimeout(function () {
                // Resize fullscreen elements.
                $("body").css("overflow", "hidden");

                var newWidth = $window.width();
                var newHeight = $window.height() - $header.outerHeight() - $footer.outerHeight();
                $(".content .slider").css('height', newHeight > settings.minHeight ? newHeight : settings.minHeight);
                $(".content .slider").css('width', newWidth > settings.minWidth ? newWidth : settings.minWidth);
                //$(".content .cont").css('width', ((newWidth > settings.minWidth ? newWidth : settings.minWidth) * 3));
            }, 100);
        });
        // Trigger events on load.
        $window.load(function () {
            $window.trigger('resize');
        });

        $(".prevButton").bind("click", function () {
            var owl = $(".owl-carousel").data('owlCarousel');
            prev(owl);
        });

        $(".nextButton").bind("click", function () {
            var owl = $(".owl-carousel").data('owlCarousel');
            next(owl);
        });

        $(".slide_switch").bind("click", function () {
            var owl = $(".owl-carousel").data('owlCarousel');
            var slideId = $(this).attr("data-id");
            owl.goTo(slideId);
        });

        console.log($("#owl-demo").length);
        if ($("#owl-demo").length > 0) {
            $("#owl-demo").owlCarousel({
                navigation: false, // Show next and prev buttons
                pagination: false,
                slideSpeed: 300,
                paginationSpeed: 400,
                singleItem: true,
                mouseDrag: false,
                touchDrag: true,
                afterMove: after,
                startDragging: dragg,
            });
        }

        var isDragg = false;
        
        function after() {
            var owl = $(".owl-carousel").data('owlCarousel');
            markActualElementsToActive(owl);

            if (!isDragg) {
                return false;
            }

            if (owl.currentItem >= currentSlideIndex()) {
                next(owl);
            }
            else {
                prev(owl);
            }
            isDragg = false;
        }

        function markActualElementsToActive(owl) {
            $('.slide_switch').removeClass('active');
            $('.slide_switch[data-id="' + owl.currentItem + '"]').addClass('active');
            $('.slider').removeClass('active');
            $('.slider[data-id="' + owl.currentItem + '"]').addClass('active');
        }

        function dragg() {
            isDragg = true;
        }

        function prev(owl) {
            if (!isDragg) {
                owl.prev();
            }
        }

        function next(owl) {
            if (!isDragg) {
                owl.next();
            }
        }

        function currentSlideIndex() {
            var currentSlideIndex = $('.slider.active').attr('data-id');
            return currentSlideIndex != undefined && (currentSlideIndex >= 0 && currentSlideIndex <= 2) ? currentSlideIndex : 0;
        }

/*============================================================================================================================================*/
        if ($('.swiper-container').length > 0) {
          mySwiper = $('.swiper-container').swiper({
            paginationClickable: false,
            mode: 'vertical',
            onSlideChangeEnd: function (swiper) {
              $(".unit_nav").removeClass("active");
              $(".unit_nav").eq(swiper.activeIndex).addClass("active");

              if (swiper.activeIndex == mySwiper.slides.length - 1 && $(mySwiper.activeSlide()).hasClass('about')) {
                $('.block-desc').fadeIn();
              }
              else {
                $('.block-desc').fadeOut();
              }
            }
          });
        }

        console.log(mySwiper.activeSlide());

        renderNavPanel();

        $(window).resize(function () {
          showOrHidePageLogo($(".block_ins.current"));
        });

        $(".partner_logo").bind("click", function () {
          var elementIndex = $(this).attr("data-id");
          mySwiper.swipeTo(elementIndex);
        });
/*============================================================================================================================================*/

        $(document).mousewheel(function (event, delta) {
            if (delta < 0) {
                mySwiper.swipeNext()
            }

            if (delta > 0) {
                mySwiper.swipePrev()
            }
        });
    });
})(jQuery);



