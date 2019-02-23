$(document).ready(function () {
  var hiddenNav = $('.main-menu .hidden'),
      visibleNav = $('.main-menu .visible'),
      enrollForm = $('.enroll-form'),
      onlineButton = $('.o-button'),
      ACTIVE = 'active';

  visibleNav.find('a').bind('click', function () {
    var that = this;
    visibleNav.find('a').removeClass(ACTIVE);
    $(that).addClass(ACTIVE);
  });

  $('.main-menu .visible li:not(.button-cont)').bind("mouseover", function () {
    var that = this;
    hiddenNav.show();
      //При наведении курсора на элемент видимого меню, установка класса .active для соответствущего элемента скрытого меню.
      hiddenNav.find('li').eq($(that).index()).addClass(ACTIVE);
  });
  $('.main-menu .visible li:not(.button-cont)').bind("mouseleave", function () {
    hiddenNav.find('li').removeClass(ACTIVE);
  });
  $('.main-menu').bind("mouseleave", function () {
    hiddenNav.hide();
  });

  onlineButton.click(function () {
    enrollForm.toggle("slide", { direction: "up" }, 1000);
  });

  enrollForm.children('.title').click(function () {
    enrollForm.toggle("slide", { direction: "up" }, 1000);
  });

  //Установка ширины элементов скрытого меню. Ширина будет равной ширине соответствующего элемента видимого меню.
  // {
  hiddenNav.find("ul li").each(function (index, value) {
    // Заглушка - прибавляем 5px по причине того, что ширина 2 го элемента расчитывается неправильно(на 5 px меньше; видимо из-за размера шрифта английских букв)
    $(value).css('width', index == 1 ? visibleNav.find("ul li").eq(index).innerWidth() + 0 : visibleNav.find("ul li").eq(index).innerWidth());
  });
  // }
});