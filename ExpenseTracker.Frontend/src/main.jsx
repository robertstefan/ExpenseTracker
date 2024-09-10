import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './App.jsx';
import 'sweetalert2/src/sweetalert2.scss';
import '@mantine/dates/styles.css';
import '@mantine/charts/styles.css';

createRoot(document.getElementById('root')).render(
	<StrictMode>
		<App />
	</StrictMode>
);
