import { Route, Routes } from 'react-router-dom';
import Dashboard from '../pages/dashboard';
import NotFound from '../pages/notFound';
import Layout from '../components/layout';
import Categories from '../pages/categories';
import NewCategory from '../pages/categories/category/NewCategory';
import EditCategory from '../pages/categories/category/EditCategory';
import SignIn from '../pages/signin/SignIn';
import SignUp from '../pages/signup/SignUp';
import Users from '../pages/users';
import ViewUser from '../pages/users/user/ViewUser';

export default function AppRoutes() {
	return (
		<Routes>
			<Route element={<Layout />}>
				<Route index element={<Dashboard />} />
				<Route path='/category' element={<Categories />} />
				<Route path='/category/new' element={<NewCategory />} />
				<Route path='/category/:id' element={<EditCategory />} />
				<Route path='/user' element={<Users />} />
				<Route path='/user/:id' element={<ViewUser />} />
			</Route>
			<Route path='/sign-in' element={<SignIn />} />
			<Route path='/sign-up' element={<SignUp />} />
			<Route path='*' element={<NotFound />} />
		</Routes>
	);
}
