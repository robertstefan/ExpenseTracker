import { Route, Routes } from 'react-router-dom';
import Dashboard from '../pages/dashboard';
import NotFound from '../pages/notFound';

import Layout from '../components/layout';
import Categories from '../pages/categories';
import Users from '../pages/users';
import NewCategory from '../pages/categories/category/NewCategory';
import EditCategory from '../pages/categories/category/EditCategory';
import NewUser from '../pages/users/user/NewUser';
import Transactions from '../pages/transactions';

export default function AppRoutes() {
	return (
		<Routes>
			<Route element={<Layout />}>
				<Route index element={<Dashboard />} />

				<Route path='/categories' element={<Categories />} />
				<Route path='/category/new' element={<NewCategory />} />
				<Route path='/categories/:id' element={<EditCategory />} />
				<Route path='/users' element={<Users />} />
				<Route path='/users/new' element={<NewUser />} />
				<Route path='/transactions'>
					<Route index element={<Transactions />} />
				</Route>
			</Route>
			<Route path='*' element={<NotFound />} />
		</Routes>
	);
}
