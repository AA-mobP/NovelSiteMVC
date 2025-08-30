// مثال بسيط لإظهار شخصية كرتونية بعد تحميل الصفحة
// كود JavaScript محسن
document.addEventListener('DOMContentLoaded', () => {
    // عرض الشخصية المناسبة للصفحة الحالية بعد تحميل الصفحة
    setTimeout(() => {
        // التحقق مما إذا كان المستخدم قد زار الموقع من قبل
        if (!sessionStorage.getItem("characterShown")) {
            // إظهار الشخصية
            const isReturningUser = localStorage.getItem('visited');

            if (isReturningUser && getCurrentPage() === 'home') {
                showPageCharacter('happy', 'returning');
            } else {
                showPageCharacter(); // استخدام الحالة والرسالة الافتراضية
            }

            // تسجيل الزيارة
            localStorage.setItem('visited', 'true');

            // وضع علامة في session storage
            sessionStorage.setItem("characterShown", "true");
        }
        
    }, 2000);

    // يمكنك إضافة استدعاءات إضافية حسب التفاعل

    // مثال: عرض شخصية بحالة أخرى بعد بقاء المستخدم في الصفحة فترة طويلة
    setTimeout(() => {
        if (document.visibilityState === 'visible') {
            // تأكد من أن المستخدم ما زال في الصفحة
            //TODO: إضافة صفحة من نحن
            if (getCurrentPage() === 'about') {
                showPageCharacter('happy', 'history');
            } else if (getCurrentPage() !== 'other') {
                showPageCharacter('question', 'plan', 7000);
            }
        }
    }, 30000); // بعد 30 ثانية

    // 2عرض شخصية بحالة أخرى بعد بقاء المستخدم في الصفحة فترة طويلة
    setTimeout(() => {
        if (document.visibilityState === 'visible') {
            // تأكد من أن المستخدم ما زال في الصفحة
            //TODO: إضافة صفحة من نحن
            if (getCurrentPage() === 'about') {
                showPageCharacter('happy', 'history');
            } else if (getCurrentPage() !== 'other') {
                showPageCharacter('happy', 'love', 30000);
            }
        }
    }, 600000); // بعد 10 دقائق

    // 3عرض شخصية بحالة أخرى بعد بقاء المستخدم في الصفحة فترة طويلة
    setTimeout(() => {
        if (document.visibilityState === 'visible') {
            // تأكد من أن المستخدم ما زال في الصفحة
            //TODO: إضافة صفحة من نحن
            if (getCurrentPage() === 'about') {
                showPageCharacter('happy', 'history');
            } else if (getCurrentPage() !== 'other') {
                showPageCharacter('happy', 'love', 60000);
            }
        }
    }, 1200000); // بعد 20 دقيقة
});

function adjustBubblePosition(character) {
    const bubble = character.querySelector('.speech-bubble');
    const rect = bubble.getBoundingClientRect();

    // التأكد من أن الفقاعة داخل الشاشة من اليسار
    if (rect.left < 10) {
        bubble.style.left = '0';
        bubble.style.transform = 'none';
    }

    // التأكد من أن الفقاعة داخل الشاشة من اليمين
    if (rect.right > window.innerWidth - 10) {
        bubble.style.left = 'auto';
        bubble.style.right = '0';
        bubble.style.transform = 'none';
    }

    // التأكد من أن الفقاعة داخل الشاشة من الأعلى
    if (rect.top < 10) {
        // تغيير موضع الفقاعة لتكون أسفل الشخصية
        bubble.style.top = 'auto';
        bubble.style.bottom = '-80px';

        // تعديل سهم الفقاعة ليشير للأعلى
        bubble.classList.add('bubble-bottom');
    } else {
        // إعادة الفقاعة لوضعها الطبيعي فوق الشخصية
        bubble.style.top = '-80px';
        bubble.style.bottom = 'auto';

        // إعادة سهم الفقاعة للإشارة للأسفل
        bubble.classList.remove('bubble-bottom');
    }
}

// تخزين خيار إظهار الشخصيات
function toggleCharacters(enabled) {
    localStorage.setItem('showCharacters', enabled);
}

function shouldShowCharacters() {
    return localStorage.getItem('showCharacters') !== 'false';
}

function addCharacterToggleButton() {
    const toggleBtn = document.createElement('button');
    toggleBtn.className = 'character-toggle';
    toggleBtn.innerHTML = shouldShowCharacters() ? 'إيقاف المساعد' : 'تفعيل المساعد';
    toggleBtn.addEventListener('click', () => {
        const newState = !shouldShowCharacters();
        toggleCharacters(newState);
        toggleBtn.innerHTML = newState ? 'إيقاف المساعد' : 'تفعيل المساعد';

        if (newState) {
            showPageCharacter('default', 'enable');
        } else {
            const existingCharacter = document.querySelector('.animated-character');
            if (existingCharacter) {
                existingCharacter.remove();
            }
        }
    });
    document.body.appendChild(toggleBtn);
}

// أضف هذا الزر في زاوية الصفحة

// متغيرات للتحكم في حالة النشاط والمؤقت
let inactivityTimer;
let userWasInactive = false;

// دالة لإعادة ضبط مؤقت عدم النشاط
function resetInactivityTimer() {
    // إذا كان المستخدم كان غير نشط وعاد للتفاعل مع الصفحة
    if (userWasInactive) {
        // استدعاء دالة العودة للنشاط
        handleUserReturnedActive();
        // إعادة تعيين حالة عدم النشاط
        userWasInactive = false;
    }

    // إعادة ضبط المؤقت
    clearTimeout(inactivityTimer);
    inactivityTimer = setTimeout(() => {
        // تحديد المستخدم على أنه غير نشط
        userWasInactive = true;

        // إظهار الشخصية عند عدم النشاط (إذا كان يجب إظهارها)
        if (shouldShowCharacters()) {
            showPageCharacter('question', 'inactive', 30000);
        }
    }, 120000); // بعد دقيقتين من عدم النشاط
}

// دالة تُنفذ عندما يعود المستخدم للنشاط بعد فترة عدم نشاط
function handleUserReturnedActive() {

    // هنا يمكنك إضافة أي إجراء تريده عند عودة المستخدم للنشاط
    // مثال: إظهار شخصية مرحبة أو رسالة
    if (shouldShowCharacters()) {
        showPageCharacter('happy', 'active');
    }

    // أو تنفيذ أي منطق آخر حسب احتياجك
    // مثال: refreshUserSession();
}

// إضافة مستمعات أحداث لإعادة ضبط المؤقت عند أي نشاط
['mousemove', 'keypress', 'scroll', 'click'].forEach(event => {
    document.addEventListener(event, resetInactivityTimer);
});

// بدء تشغيل المؤقت عند تحميل الصفحة
resetInactivityTimer();
// تحديث الفقاعة النصية بتأثير متحرك
function updateBubbleText(character, newMessage) {
    const bubble = character.querySelector('.speech-bubble');

    // تلاشي الفقاعة
    bubble.style.opacity = '0';

    setTimeout(() => {
        bubble.textContent = newMessage;

        // ضبط موضع الفقاعة بعد تغيير النص
        adjustBubblePosition(character);

        // إظهار الفقاعة مرة أخرى
        bubble.style.opacity = '1';
    }, 300);
}

//TODO: إضافة تعليمات لملئ حقول النموذج
// مثال لمراقبة النموذج وإظهار الشخصية عند الحاجة
//function watchForms() {
//    document.querySelectorAll('form').forEach(form => {
//        const formFields = form.querySelectorAll('input, textarea, select');

//        formFields.forEach(field => {
//            field.addEventListener('focus', () => {
//                // تحقق مما إذا كان الحقل فارغًا وكان له تسمية
//                const fieldLabel = form.querySelector(`label[for="${field.id}"]`);
//                if (!field.value && fieldLabel && shouldShowCharacters()) {
//                    showPageCharacter('helper', `أدخل ${fieldLabel.textContent} هنا`);//won't work
//                }
//            });
//        });

//        form.addEventListener('submit', () => {
//            if (shouldShowCharacters()) {
//                showPageCharacter('celebration', 'تم إرسال النموذج بنجاح!');//won't work
//            }
//        });
//    });
//}

// كود للتعرف على الصفحة الحالية
function getCurrentPage() {
    // يمكنك تعديل هذه الدالة لتناسب هيكل موقعك
    const path = window.location.pathname;

    if (path === '/' || path.toLowerCase() === '/home' || path.toLowerCase().includes('/websiteblog') || path.toLowerCase().includes('/todo')) {
        return 'home';
    } else if (path.toLowerCase().includes('/admin')) {
        return 'admin';
    }
    // أضف المزيد من الصفحات حسب حاجتك

    return 'other'; // للصفحات الأخرى
}

// تكوين أنواع الشخصيات وحالاتها لكل صفحة
const characterConfig = {
    home: {
        type: 'assistant',
        states: {
            default: 'welcome', // الحالة الافتراضية
            happy: 'excited',
            question: 'curious'
        },
        messages: {
            default: 'مرحباً بك في موقعك سيدتي مينه!',
            returning: 'مرحبا بعودتك سيدتي مينه!',
            help: 'هل لديك أي أفكار جديدة للموقع سيدتي؟',
            enable: 'ها قد عدت للعمل، كيف أساعدك سيدتي؟',
            active: 'آه ها قد عادت السيدة مينه!',
            plan: 'ما الذي تخططين لفعله اليوم سيدة مينه؟',
            inactive: 'هل من أحدٍ هنا؟',
            love: 'أحببببببببك سيدتي مينه أحببببك كثيرا يا قمري!'
        }
    }
    //إضافة صفحات أخرى لتوسع الشخصيات
    /*,blog: {
        type: 'blogger',
        states: {
            default: 'reading',
            happy: 'sharing',
            question: 'thinking'
        },
        messages: {
            default: 'استمتع بقراءة منشوراتنا!',
            suggestion: 'ما رأيك أن تجرب منشورا على ذوقنا؟ اضغط على زر فاجئني!',
            popular: 'هذه هي المقالات الأكثر شهرة لدينا!'
        }
    }*/
};

// دالة لعرض الشخصية المناسبة للصفحة الحالية
function showPageCharacter(state = 'default', messageKey = 'default', timer = 7000) {
    const currentPage = getCurrentPage();

    // التحقق من أن الصفحة الحالية مدعومة
    if (!characterConfig[currentPage]) {
        return; // لا توجد شخصية مخصصة لهذه الصفحة
    }

    const config = characterConfig[currentPage];
    const characterType = config.type;
    const characterState = config.states[state] || config.states.default;
    const message = config.messages[messageKey] || config.messages.default;

    // استدعاء الدالة الرئيسية مع الحالة المناسبة
    showCharacterWithState(characterType, message, characterState, timer);
}

// دالة محسنة لعرض شخصية بحالة محددة
function showCharacterWithState(type, message, state = 'default', timer) {
    // إزالة أي شخصية موجودة سابقاً
    const existingCharacter = document.querySelector('.animated-character');
    if (existingCharacter) {
        existingCharacter.remove();
    }

    const character = document.createElement('div');
    character.className = `animated-character ${type}-character`;
    character.innerHTML = `
        <div class="speech-bubble">${message}</div>
        <img src="assets/image/characters/${type}-${state}.webp" alt="شخصية ${type}">
        <button class="close-character" aria-label="إغلاق">×</button>
    `;
    document.body.appendChild(character);

    // ضبط موضع الفقاعة بناءً على حجم الشاشة
    adjustBubblePosition(character);

    // إظهار الشخصية بتأثير متحرك
    setTimeout(() => {
        character.classList.add('active');

        // إضافة مؤقت لإخفاء الشخصية بعد 7 ثواني
        const autoHideTimer = setTimeout(() => {
            character.classList.remove('active');
            setTimeout(() => {
                character.remove();
            }, 500);
        }, timer); // الافتراضي 7000 مللي ثانية = 7 ثواني

        // تخزين المؤقت كخاصية للعنصر للتمكن من إلغائه إذا تم الضغط على زر الإغلاق
        character.autoHideTimer = autoHideTimer;
    }, 100);

    // تعديل حدث الإغلاق
    character.querySelector('.close-character').addEventListener('click', () => {
        // إلغاء المؤقت التلقائي لتجنب الاستدعاء المزدوج
        clearTimeout(character.autoHideTimer);

        character.classList.remove('active');
        setTimeout(() => {
            character.remove();
        }, 500);
    });

    // تعديل موضع الفقاعة عند تغيير حجم النافذة
    window.addEventListener('resize', () => {
        adjustBubblePosition(character);
    });
}

// مثال: إظهار شخصية عند النقر على زر محدد
function setupCharacterTriggers() {
    // زر المساعدة في الصفحة الرئيسية
    const helpButton = document.querySelector('.help-button');
    if (helpButton) {
        helpButton.addEventListener('click', () => {
            showPageCharacter('question', 'help');
        });
    }

    //// زر البحث في صفحة المنشورات
    //const searchButton = document.querySelector('.search-button');
    //if (searchButton && getCurrentPage() === 'posts') {
    //    searchButton.addEventListener('click', () => {
    //        showPageCharacter('happy', 'suggestion');
    //    });
    //}

    //// نماذج الاتصال
    //const contactForm = document.querySelector('.contact-form');
    //if (contactForm && getCurrentPage() === 'contact') {
    //    contactForm.addEventListener('submit', (e) => {
    //        e.preventDefault(); // منع إرسال النموذج (للعرض فقط)
    //        showPageCharacter('happy', 'form');
    //    });
    //}
}

// استدعاء دالة إعداد مشغلات الشخصيات
document.addEventListener('DOMContentLoaded', setupCharacterTriggers);

// تحميل الصور مسبقاً لتجنب التأخير عند العرض
function preloadCharacterImages() {
    // حلقة على جميع أنواع الشخصيات والحالات
    Object.values(characterConfig).forEach(config => {
        const type = config.type;
        Object.values(config.states).forEach(state => {
            const img = new Image();
            img.src = `assets/image/characters/${type}-${state}.webp`;
        });
    });
}

// استدعاء التحميل المسبق بعد تحميل الصفحة
window.addEventListener('load', preloadCharacterImages);

