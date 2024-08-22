import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import { AppShell, Box } from '@mantine/core';
// import Header from '../header';
import Navbar from '../navbar';

function Layout() {
	const [opened, setOpened] = useState(false);

	return (
		<AppShell padding='md' navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: !opened } }}>

			<AppShell.Navbar>
				<Navbar />
			</AppShell.Navbar>
			<AppShell.Main>
				<Box>
					<div>
						<Outlet />
					</div>
				</Box>
			</AppShell.Main>
		</AppShell>
	);
}

export default Layout;
