import { Route, Routes } from 'react-router-dom';
import Dashboard from '../pages/dashboard';
import NotFound from '../pages/notFound';
import Categories from '../pages/categories';
import Layout from '../components/layout';
import NewCategory from '../pages/categories/category/NewCategory';
import EditCategory from '../pages/categories/category/EditCategory';

export default function AppRoutes() {
	return (
		<Routes>
			<Route element={<Layout />}>
				<Route index element={<Dashboard />} />
				<Route path='/category' element={<Categories />} />
				<Route path='/category/new' element={<NewCategory />} />
				<Route path='/category/:id' element={<EditCategory />} />
			</Route>
			<Route path='*' element={<NotFound />} />
		</Routes>
	);
}
