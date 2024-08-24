import { Route, Routes } from 'react-router-dom';
import Dashboard from '../pages/dashboard';
import NotFound from '../pages/notFound';
import Layout from '../components/layout';
import Categories from '../pages/categories';
import NewCategory from '../pages/categories/category/NewCategory';
import EditCategory from '../pages/categories/category/EditCategory';

export default function AppRoutes() {
	return (
		<Routes>
			<Route element={<Layout />}>
				<Route index element={<Dashboard />} />
				<Route path='/categories' element={<Categories />} />
				<Route path='/category/new' element={<NewCategory />} />
				<Route path='/categories/:id' element={<EditCategory />} />
			</Route>
			<Route path='*' element={<NotFound />} />
		</Routes>
	);
}
