// تنفيذ تأثير انتقال الصفحة
function initPageTransition() {
    // إنشاء عنصر تأثير الانتقال
    const pageTransition = document.createElement('div');
    pageTransition.id = 'pageTransition';
    pageTransition.className = 'page-transition slide-in-right';
    document.body.appendChild(pageTransition);

    // إضافة فئة fade-in للمحتوى الرئيسي
    const mainContent = document.querySelector('main') || document.querySelector('.content') || document.body;
    mainContent.classList.add('fade-in');

    // بعد انتهاء تأثير الدخول، نبدأ تأثير الخروج
    setTimeout(() => {
        pageTransition.classList.remove('slide-in-right');
        pageTransition.classList.add('slide-out-left');
    }, 1000);
}

// تسجيل الدالة للتشغيل عند تحميل الصفحة
document.addEventListener('DOMContentLoaded', initPageTransition);

// حفظ حالة الانتقال قبل مغادرة الصفحة
window.addEventListener('beforeunload', function () {
    // يمكن إضافة رمز إضافي هنا لتحسين تجربة الانتقال بين الصفحات
    localStorage.setItem('pageTransition', 'true');
});