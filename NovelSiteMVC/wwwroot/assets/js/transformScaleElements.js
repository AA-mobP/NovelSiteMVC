// في ملف site.js أو script.js الخاص بك
// wwwroot/js/site.js

// تطبيق تأثير النبض على جميع العناصر القابلة للنقر
document.addEventListener('DOMContentLoaded', function () {
    // اختيار جميع العناصر التي تقبل النقر
    const clickableElements = document.querySelectorAll('a, button, input, select, label, h1, h2, h3, h4, h5, h6, [role="button"], [onclick]');
    //const clickableElements = document.querySelectorAll('a, button, input[type="button"], input[type="submit"], input[type="reset"], input[type="text"], select, [role="button"], [onclick]');

    // إضافة فئة التأثير لكل عنصر
    clickableElements.forEach(element => {
        element.classList.add('pulse-effect');
    });
});