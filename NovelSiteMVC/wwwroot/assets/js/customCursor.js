// Custom Cursor with Element Highlighting, Scroll Support, and Melting Effect
document.addEventListener('DOMContentLoaded', function () {
    // Create cursor elements
    const cursor = document.createElement('div');
    const cursorFollower = document.createElement('div');
    const elementHighlight = document.createElement('div');

    // Add elements to the body
    document.body.appendChild(cursor);
    document.body.appendChild(cursorFollower);
    document.body.appendChild(elementHighlight);

    // Style the main cursor
    cursor.style.cssText = `
        position: fixed;
        width: 12px;
        height: 12px;
        background-color: #3498db;
        border-radius: 50%;
        pointer-events: none;
        z-index: 9999;
        transition: transform 0.1s ease;
        transform: translate(-50%, -50%);
        box-shadow: 0 0 8px 2px rgba(52, 152, 219, 0.8);
    `;

    // Style the cursor follower with darker border and bright glow
    cursorFollower.style.cssText = `
        position: fixed;
        width: 45px;
        height: 45px;
        background-color: rgba(52, 152, 219, 0.15);
        border: 3px solid rgba(25, 75, 125, 0.9);
        border-radius: 50%;
        pointer-events: none;
        z-index: 9998;
        transition: transform 0.2s ease, left 0.15s ease-out, top 0.15s ease-out;
        transform: translate(-50%, -50%);
        box-shadow: 0 0 15px 5px rgba(52, 152, 219, 0.7);
    `;

    // Style the element highlight (initially hidden)
    elementHighlight.style.cssText = `
        position: fixed; /* Changed from absolute to fixed for scroll support */
        pointer-events: none;
        z-index: 9997;
        border: 3px solid rgba(231, 76, 60, 0.9);
        background-color: rgba(231, 76, 60, 0.1);
        box-shadow: 0 0 15px 5px rgba(231, 76, 60, 0.5);
        transition: all 0.2s ease;
        opacity: 0;
        border-radius: 4px;
    `;

    // Hide default cursor
    document.body.style.cursor = 'none';

    // Variables for melting effect
    let meltingTimer = null;
    let meltingStage = 0;
    let isMouseDown = false;
    let currentElement = null;
    let lastScrollPosition = window.pageYOffset || document.documentElement.scrollTop;

    // Make all clickable elements show the pointer on hover
    const clickableElements = document.querySelectorAll('a, button, input, textarea, select, [role="button"], [onclick]');

    function updateElementHighlight(element) {
        if (element) {
            const rect = element.getBoundingClientRect();
            const padding = 4; // padding around the element

            // Get current scroll position
            const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            const scrollLeft = window.pageXOffset || document.documentElement.scrollLeft;

            // Use fixed positioning with getBoundingClientRect values (which are relative to viewport)
            elementHighlight.style.left = (rect.left - padding) + 'px';
            elementHighlight.style.top = (rect.top - padding) + 'px';
            elementHighlight.style.width = (rect.width + padding * 2) + 'px';
            elementHighlight.style.height = (rect.height + padding * 2) + 'px';
            elementHighlight.style.opacity = '1';
            elementHighlight.style.borderRadius = getComputedStyle(element).borderRadius;

            // Hide the regular follower
            cursorFollower.style.opacity = '0';
        } else {
            elementHighlight.style.opacity = '0';
            cursorFollower.style.opacity = '1';
        }
    }

    clickableElements.forEach(el => {
        el.style.cursor = 'none';

        el.addEventListener('mouseover', () => {
            cursor.style.transform = 'translate(-50%, -50%) scale(1.5)';
            cursor.style.backgroundColor = '#e74c3c';
            cursor.style.boxShadow = '0 0 8px 2px rgba(231, 76, 60, 0.8)';

            // Set current element and update highlight
            currentElement = el;
            updateElementHighlight(el);
        });

        el.addEventListener('mouseout', () => {
            currentElement = null;
            updateElementHighlight(null);
            resetCursorStyle();
        });
    });

    // Function to reset cursor style
    function resetCursorStyle() {
        if (!isMouseDown) {
            cursor.style.transform = 'translate(-50%, -50%) scale(1)';
            cursor.style.backgroundColor = '#3498db';
            cursor.style.boxShadow = '0 0 8px 2px rgba(52, 152, 219, 0.8)';

            cursorFollower.style.transform = 'translate(-50%, -50%) scale(1)';
            cursorFollower.style.backgroundColor = 'rgba(52, 152, 219, 0.15)';
            cursorFollower.style.borderColor = 'rgba(25, 75, 125, 0.9)';
            cursorFollower.style.boxShadow = '0 0 15px 5px rgba(52, 152, 219, 0.7)';

            // Reset any melting effects
            cursorFollower.style.borderRadius = '50%';
            cursorFollower.style.width = '45px';
            cursorFollower.style.height = '45px';
            cursorFollower.style.opacity = currentElement ? '0' : '1';
        }
    }

    // Handle scroll events to update element highlight
    window.addEventListener('scroll', () => {
        if (currentElement) {
            updateElementHighlight(currentElement);
        }

        // Store current scroll position for potential calculations
        lastScrollPosition = window.pageYOffset || document.documentElement.scrollTop;
    });

    // Update cursor position with smoother follower movement
    document.addEventListener('mousemove', e => {
        cursor.style.left = e.clientX + 'px';
        cursor.style.top = e.clientY + 'px';

        // Only move the follower if we're not hovering over a clickable element
        if (!currentElement) {
            // Follower with slight delay but more natural movement
            setTimeout(() => {
                if (!isMouseDown || meltingStage < 3) {
                    cursorFollower.style.left = e.clientX + 'px';
                    cursorFollower.style.top = e.clientY + 'px';
                }
            }, 40);
        }

        // Check if the element under cursor has changed (for nested elements)
        const elementUnderCursor = document.elementFromPoint(e.clientX, e.clientY);
        let isClickable = false;

        if (elementUnderCursor) {
            // Check if the element or any of its parents are clickable
            let element = elementUnderCursor;
            while (element) {
                if (element.tagName === 'A' ||
                    element.tagName === 'BUTTON' ||
                    element.tagName === 'INPUT' ||
                    element.tagName === 'TEXTAREA' ||
                    element.tagName === 'SELECT' ||
                    element.getAttribute('role') === 'button' ||
                    element.hasAttribute('onclick')) {
                    isClickable = true;

                    // Only update if it's a different element
                    if (currentElement !== element) {
                        currentElement = element;
                        updateElementHighlight(element);

                        cursor.style.transform = 'translate(-50%, -50%) scale(1.5)';
                        cursor.style.backgroundColor = '#e74c3c';
                        cursor.style.boxShadow = '0 0 8px 2px rgba(231, 76, 60, 0.8)';
                    }
                    break;
                }
                element = element.parentElement;
            }
        }

        // If not over a clickable element anymore
        if (!isClickable && currentElement) {
            currentElement = null;
            updateElementHighlight(null);
            resetCursorStyle();
        }
    });

    // Melting effect on long press
    document.addEventListener('mousedown', (event) => {
        isMouseDown = true;
        cursor.style.transform = 'translate(-50%, -50%) scale(0.8)';

        if (!currentElement) {
            cursorFollower.style.transform = 'translate(-50%, -50%) scale(0.9)';
        } else {
            // Add a pressed effect to the element highlight
            elementHighlight.style.boxShadow = '0 0 10px 3px rgba(231, 76, 60, 0.8), inset 0 0 5px rgba(231, 76, 60, 0.5)';
            elementHighlight.style.transform = 'scale(0.98)';
        }

        // Create click ripple effect
        const ripple = document.createElement('div');
        ripple.style.cssText = `
            position: fixed;
            left: ${event.clientX}px;
            top: ${event.clientY}px;
            width: 20px;
            height: 20px;
            background-color: rgba(52, 152, 219, 0.2);
            border: 3px solid rgba(25, 75, 125, 0.9);
            border-radius: 50%;
            transform: translate(-50%, -50%);
            animation: rippleEffect 0.7s ease-out;
            pointer-events: none;
            z-index: 9996;
            box-shadow: 0 0 12px 3px rgba(52, 152, 219, 0.6);
        `;

        document.body.appendChild(ripple);

        setTimeout(() => {
            document.body.removeChild(ripple);
        }, 700);

        // Only start melting if not over a clickable element
        if (!currentElement) {
            // Start melting timer
            meltingStage = 0;
            clearInterval(meltingTimer);
            meltingTimer = setInterval(() => {
                meltingStage++;

                if (meltingStage === 1) {
                    // Start melting - become slightly oval
                    cursorFollower.style.borderRadius = '45% 45% 50% 50%';
                    cursorFollower.style.height = '50px';
                } else if (meltingStage === 2) {
                    // More melting - more oval, dripping
                    cursorFollower.style.borderRadius = '40% 40% 55% 55%';
                    cursorFollower.style.height = '60px';
                    cursorFollower.style.width = '43px';
                } else if (meltingStage === 3) {
                    // Puddle forming
                    cursorFollower.style.borderRadius = '35% 35% 60% 60%';
                    cursorFollower.style.height = '70px';
                    cursorFollower.style.width = '40px';
                } else if (meltingStage === 4) {
                    // More puddle-like
                    cursorFollower.style.borderRadius = '30% 30% 70% 65%';
                    cursorFollower.style.height = '85px';
                    cursorFollower.style.width = '38px';
                } else if (meltingStage === 5) {
                    // Almost completely melted
                    cursorFollower.style.borderRadius = '25% 25% 75% 75%';
                    cursorFollower.style.height = '100px';
                    cursorFollower.style.width = '35px';
                    cursorFollower.style.opacity = '0.8';
                }
                else if (meltingStage === 6) {
                    // Final puddle
                    cursorFollower.style.borderRadius = '40% 40% 80% 80%';
                    cursorFollower.style.height = '110px';
                    cursorFollower.style.width = '30px';
                    cursorFollower.style.opacity = '0.6';
                } else if (meltingStage === 7) {
                    // Start disappearing
                    cursorFollower.style.opacity = '0.4';
                } else if (meltingStage === 8) {
                    // Almost gone
                    cursorFollower.style.opacity = '0.2';
                } else if (meltingStage >= 9) {
                    // Complete melt, stop the interval
                    cursorFollower.style.opacity = '0';
                    clearInterval(meltingTimer);
                }
            }, 125); // Change melting stages every 75ms
        }
    });

    document.addEventListener('mouseup', () => {
        isMouseDown = false;
        clearInterval(meltingTimer);

        cursor.style.transform = currentElement ? 'translate(-50%, -50%) scale(1.5)' : 'translate(-50%, -50%) scale(1)';

        if (currentElement) {
            // Restore the element highlight
            elementHighlight.style.boxShadow = '0 0 15px 5px rgba(231, 76, 60, 0.5)';
            elementHighlight.style.transform = 'scale(1)';
        } else {
            // Animation for reforming from melted state
            if (meltingStage > 0) {
                cursorFollower.style.transition = 'all 0.4s ease-in-out';
                resetCursorStyle();

                // Reset the transition after reforming animation completes
                setTimeout(() => {
                    cursorFollower.style.transition = 'transform 0.2s ease, left 0.15s ease-out, top 0.15s ease-out';
                }, 400);
            } else {
                resetCursorStyle();
            }
        }
    });

    // Text selection effect
    document.addEventListener('selectstart', () => {
        if (!currentElement) {
            cursor.style.backgroundColor = '#9b59b6';
            cursor.style.boxShadow = '0 0 8px 2px rgba(155, 89, 182, 0.8)';

            cursorFollower.style.backgroundColor = 'rgba(155, 89, 182, 0.15)';
            cursorFollower.style.borderColor = 'rgba(100, 40, 120, 0.9)';
            cursorFollower.style.boxShadow = '0 0 15px 5px rgba(155, 89, 182, 0.7)';
        }
    });

    document.addEventListener('selectionchange', () => {
        if (document.getSelection().toString().length > 0 && !currentElement) {
            cursor.style.backgroundColor = '#9b59b6';
            cursor.style.boxShadow = '0 0 8px 2px rgba(155, 89, 182, 0.8)';

            cursorFollower.style.backgroundColor = 'rgba(155, 89, 182, 0.15)';
            cursorFollower.style.borderColor = 'rgba(100, 40, 120, 0.9)';
            cursorFollower.style.boxShadow = '0 0 15px 5px rgba(155, 89, 182, 0.7)';
        } else if (!isMouseDown && !currentElement) {
            resetCursorStyle();
        }
    });

    // Make cursor visible when document is inactive but mouse is over window
    document.addEventListener('mouseleave', () => {
        cursor.style.opacity = '0';
        cursorFollower.style.opacity = '0';
        elementHighlight.style.opacity = '0';
    });

    document.addEventListener('mouseenter', () => {
        cursor.style.opacity = '1';
        cursorFollower.style.opacity = currentElement ? '0' : '1';
        elementHighlight.style.opacity = currentElement ? '1' : '0';
    });

    // Handle window resize to recalculate element highlight positions
    window.addEventListener('resize', () => {
        if (currentElement) {
            updateElementHighlight(currentElement);
        }
    });

    // Add ripple animation
    const style = document.createElement('style');
    style.textContent = `
        @keyframes rippleEffect {
            0% {
                width: 20px;
                height: 20px;
                opacity: 1;
                border-width: 3px;
            }
            100% {
                width: 120px;
                height: 120px;
                opacity: 0;
                border-width: 1px;
            }
        }
    `;
    document.head.appendChild(style);
});