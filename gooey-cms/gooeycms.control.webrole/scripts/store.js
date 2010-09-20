var $j = jQuery.noConflict();
$j(function () {
    var jThumbs = $j('ul.thumbs');
    var jPager = jThumbs.parent().find('ul.thumb-nav');
    jThumbs.each(
		function () {
		    var jThis = $j(this);
		    var jParent = jThis.parent();
		    var jPager = jParent.find('ul.thumb-nav');
		    var jNext = jParent.find('div.next');
		    var jPrev = jParent.find('div.prev');
		    jThis.cycle({
		        fx: 'scrollHorz',
		        speed: 500,
		        pager: jPager,
		        pagerAnchorBuilder: function (index, DOMelement) {
		            return '<li><a href="#"></a></li>';
		        },
		        timeout: 0,
		        activePagerClass: 'active',
		        pagerClick: function (zeroBasedSlideIndex, slideElement) { },
		        pagerEvent: 'click.cycle',
		        next: jNext,
		        prev: jPrev

		    });

		}
	);
});