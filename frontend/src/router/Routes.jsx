import { Route, Routes } from 'react-router-dom';
import Dashboard from '../pages/dashboard';
import NotFound from '../pages/notFound';
import Categories from '../pages/categories';
import Layout from '../components/layout';
import NewCategory from '../pages/categories/category/NewCategory';
import EditCategory from '../pages/categories/category/EditCategory';
import Transactions from '../pages/transactions';
import EditTransaction from '../pages/transactions/transaction/EditTransaction';
import NewTransaction from '../pages/transactions/transaction/NewTransaction';

export default function AppRoutes() {
	return (
		<Routes>
			<Route element={<Layout />}>
				<Route index element={<Dashboard />} />
				<Route path='/categories'>
					<Route index element={<Categories />} />
					<Route path='new' element={<NewCategory />} />
					<Route path=':id' element={<EditCategory />} />
				</Route>
				<Route path='/transactions'>
					<Route index element={<Transactions />} />
					<Route path='new' element={<NewTransaction />} />
					<Route path=':id' element={<EditTransaction />} />
				</Route>
			</Route>
			<Route path='*' element={<NotFound />} />
		</Routes>
	);
}
