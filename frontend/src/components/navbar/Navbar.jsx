import { useState } from 'react';
import { Link } from 'react-router-dom';
import { Group, Code, Text } from '@mantine/core';
import { IconLogout, IconDashboard, IconCategory, IconUsers, IconTransactionBitcoin } from '@tabler/icons-react';
import classes from './Navbar.module.css';

const data = [
	{ link: '/', label: 'Dashboard', icon: IconDashboard },
	{ link: '/categories', label: 'Categories', icon: IconCategory },
	{ link: '/users', label: 'Users', icon: IconUsers },
	{ link: '/transactions', label: 'Transactions', icon: IconTransactionBitcoin },
];

export default function Navbar() {
	const [active, setActive] = useState('Dashboard');

	const links = data.map((item) => (
		<Link
			className={classes.link}
			data-active={item.label === active || undefined}
			to={item.link}
			key={item.label}
			onClick={() => {
				setActive(item.label);
			}}
		>
			<item.icon className={classes.linkIcon} stroke={1.5} />
			<span>{item.label}</span>
		</Link>
	));

	return (
		<nav className={classes.navbar}>
			<div className={classes.navbarMain}>
				<Group className={classes.header} justify='space-between'>
					<Text c='#fff'>Expense Tracker</Text>
					<Code fw={700} className={classes.version}>
						v1.0.0
					</Code>
				</Group>
				{links}
			</div>

			<div className={classes.footer}>
				<a href='#' className={classes.link} onClick={(event) => event.preventDefault()}>
					<IconLogout className={classes.linkIcon} stroke={1.5} />
					<span>Logout</span>
				</a>
			</div>
		</nav>
	);
}
