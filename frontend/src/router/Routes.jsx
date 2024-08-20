import { Route, Routes } from 'react-router-dom';
import Dashboard from '../pages/dashboard';
import NotFound from '../pages/notFound';
import Layout from '../components/layout';

export default function AppRoutes() {
	return (
		<Routes>
			<Route element={<Layout />}>
				<Route index element={<Dashboard />} />
			</Route>
			<Route path='*' element={<NotFound />} />
		</Routes>
	);
}
