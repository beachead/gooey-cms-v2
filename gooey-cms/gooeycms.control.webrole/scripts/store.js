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
		        prev: jPrev,
		        after: function (currSlideElement, nextSlideElement, options, forwardFlag) {
		            currSlideElement.parentNode.style.overflow = 'visible';
		        }
		    });

		}
	).css('overflow', 'visible'); // so that the "delete" button can be positioned beyond the boundary of its container

    $j('#themes-panel').delegate('a.showFeatures', 'hover', function () {
        var jTrigger = $j(this),
        jFeature = jTrigger.parent(),
        jList = jFeature.find('ul');
        if (jList.is(':visible')) {
            jList.slideUp();
        } else {
            jList.slideDown();
        }
    });


    $j('#themes-panel').delegate('a.addScreenshot', 'click', function () {
        $j('#ss_' + $j(this).attr('href')).slideDown('fast');
        return false;
    });

    $j('.ss_cancel').bind('click', function () {
        $j(this).parent().slideUp('fast');
        return false;
    });

});