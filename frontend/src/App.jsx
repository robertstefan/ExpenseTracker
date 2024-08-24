import { Suspense } from 'react';
import { Provider } from 'react-redux';
import { LoadingOverlay, MantineProvider } from '@mantine/core';
import { ModalsProvider } from '@mantine/modals';
import { Notifications } from '@mantine/notifications';
import Router from './router';
import store from './state/store';
import theme from './assets/styles/theme';
import Loading from './components/loading';
import '@mantine/core/styles.layer.css';
import '@mantine/notifications/styles.layer.css';
import './assets/css/main.scss';

function App() {
	return (
		<Suspense fallback={<Loading />}>
			<Provider store={store}>
				<MantineProvider theme={theme}>
					<Notifications position={'top-right'} />
					<ModalsProvider>
						<Suspense fallback={<LoadingOverlay overlayProps={{ blur: 2 }} />}>
							<Router />
						</Suspense>
					</ModalsProvider>
				</MantineProvider>
			</Provider>
		</Suspense>
	);
}

export default App;
