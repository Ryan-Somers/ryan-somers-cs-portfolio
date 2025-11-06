/** @type {import('tailwindcss').Config} */
module.exports = {

    content: [
        './Views/**/*.cshtml',
        './Pages/**/*.cshtml',
        './wwwroot/**/*.js'
    ],
    theme: {
        extend: {},
    },
    daisyui: {
        themes: ["light", "dark"],
    },
    plugins: [
        require('daisyui'),
    ],
    fontFamily: {
        'body': [
            'Inter',
            'ui-sans-serif',
            'system-ui',
            '-apple-system',
            'system-ui',
            'Segoe UI',
            'Roboto',
            'Helvetica Neue',
            'Arial',
            'Noto Sans',
            'sans-serif',
            'Apple Color Emoji',
            'Segoe UI Emoji',
            'Segoe UI Symbol',
            'Noto Color Emoji'
        ],
        'sans': [
            'Inter',
            'ui-sans-serif',
            'system-ui',
            '-apple-system',
            'system-ui',
            'Segoe UI',
            'Roboto',
            'Helvetica Neue',
            'Arial',
            'Noto Sans',
            'sans-serif',
            'Apple Color Emoji',
            'Segoe UI Emoji',
            'Segoe UI Symbol',
            'Noto Color Emoji'
        ],
    },
};

