import { Title, Text, Button, Container, Group } from '@mantine/core';
import { Link } from 'react-router-dom';
import classes from './NotFound.module.scss';

function NotFound() {
	return (
		<Container className={classes.root}>
			<div className={classes.label}>404</div>
			<Title className={classes.title}>Secret place</Title>
			<Text size='lg' ta='center' className={classes.description}>
				404
			</Text>
			<Group justify='center'>
				<Link to='/'>
					<Button size='md'>Home</Button>
				</Link>
			</Group>
		</Container>
	);
}

export default NotFound;
