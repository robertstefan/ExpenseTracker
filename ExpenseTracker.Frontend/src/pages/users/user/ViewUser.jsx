import React from 'react';
import { useParams } from 'react-router-dom';
import { Container, Group, Title, SimpleGrid, Grid } from '@mantine/core';
import UserData from './components/UserData';
import UserTransactions from './components/UserTransactions';
import UserFinancialSummary from './components/UserFinancialSummary';
import TopCategories from './components/TopCategories';
import { RaportTimespanProvider, useRaportTimespan } from '../../../context/RaportTimespanContext';
import NewTransaction from './components/NewTransaction';
const ViewUser = () => {
	const { id } = useParams();
	return (
		<RaportTimespanProvider>
			<Group>
				<Title>User Manager</Title>
			</Group>
			<Container my='xl' size={'lg'}>
				<SimpleGrid cols={{ base: 1, sm: 2 }} spacing='md'>
					<UserTransactions id={id} />
					<Grid gutter='md'>
						<Grid.Col>
							<NewTransaction id={id} />

							<UserData id={id} />
						</Grid.Col>
						<UserFinancialSummary id={id} />
					</Grid>
				</SimpleGrid>
				<TopCategories id={id} />
			</Container>
		</RaportTimespanProvider>
	);
};

export default ViewUser;
