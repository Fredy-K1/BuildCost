/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
    "./node_modules/flowbite/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        page: '#F5F4F0',
        surface: '#FFFFFF',
        surface2: '#EDEBE5',
        line: '#E0DDD6',
        ink: '#1A1814',
        muted: '#7C7770',
        accent: {
          DEFAULT: '#1E3A5F',
          light: '#E6EEF6',
        },
        warn: {
          DEFAULT: '#925A10',
          light: '#FEF3E2',
        },
        success: {
          DEFAULT: '#1A5030',
          light: '#EAF5EE',
        },
        danger: '#d93025',
      },
      fontFamily: {
        sans: ['"DM Sans"', 'sans-serif'],
        mono: ['"DM Mono"', 'monospace'],
      },
      borderRadius: {
        lg: '16px',
        DEFAULT: '10px'
      }
    },
  },
  plugins: [
    require('flowbite/plugin')
  ],
}