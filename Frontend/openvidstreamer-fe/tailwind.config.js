/** @type {import('tailwindcss').Config} */
export default {
  content: [
    './src/**/*.{js,jsx,ts,tsx}', // paths to your source files
    'node_modules/flowbite-react/lib/esm/**/*.js', // include Flowbite React components
  ],
  theme: {
    extend: {},
  },
  plugins: [
    require('flowbite/plugin'), // include Flowbite plugin
  ],
};
