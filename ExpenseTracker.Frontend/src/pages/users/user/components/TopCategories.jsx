import React, { useMemo, useState } from 'react';
import { useGetTopCategoriesQuery } from '../../../../state/raport/api';
import { Button, Grid, RingProgress, Skeleton, Stack, Table, Text, Title } from '@mantine/core';
import { useRaportTimespan } from '../../../../context/RaportTimespanContext';
const TopCategories = ({ id }) => {
	const { raportTimespan } = useRaportTimespan();

	const { data: categories = {}, isLoading, isError } = useGetTopCategoriesQuery({ userId: id, raportTimespan });

	const colors = ['#FF6F61', '#6B5B95', '#88B04B', '#F7CAC9', '#92A8D1'];

	const totalIncome = useMemo(() => {
		if (categories.items?.length > 0) return categories.items?.reduce((sum, c) => sum + c.categoryIncome, 0);
		return 0;
	}, [categories.items]);

	const totalOutcome = useMemo(() => {
		if (categories.items?.length > 0) return categories.items?.reduce((sum, c) => sum + c.categoryOutcome, 0);
		return 0;
	}, [categories.items]);

	const [mode, setMode] = useState(1);

	var ringOutcomeData = categories.items?.map((c, i) => ({
		value: Math.ceil((c.categoryOutcome / totalOutcome) * 100),
		color: colors[i],
		tooltip: `${c.name} - ${c.categoryOutcome} RON`,
	}));

	var ringIncomeData = categories.items?.map((c, i) => ({
		tooltip: `${c.name} - ${c.categoryIncome} RON`,
		value: Math.ceil((c.categoryIncome / totalIncome) * 100),
		color: colors[i],
	}));

	if (isLoading || isError)
		return (
			<Grid align='center' mt={'xl'}>
				<Grid.Col span={7}>
					<Skeleton height={500} circle />
				</Grid.Col>
				<Grid.Col span={5}>
					<Skeleton height={300} />
				</Grid.Col>
			</Grid>
		);

	return (
		<Stack align='center' mt={'xl'}>
			<div
				style={{
					marginInlineStart: 'auto',
				}}
			>
				{mode == 1 ? (
					<Button onClick={() => setMode(2)}>Switch to income</Button>
				) : (
					<Button onClick={() => setMode(1)}>Switch to outcome</Button>
				)}
			</div>
			<Grid align='center'>
				<Grid.Col span={7}>
					<RingProgress
						size={500}
						thickness={40}
						label={
							<Title order={3} ta='center' style={{ pointerEvents: 'none' }}>
								{mode == 1 ? (
									<>
										Outcome <br />
										{totalOutcome} RON
									</>
								) : (
									<>
										Income <br />
										{totalIncome} RON
									</>
								)}
							</Title>
						}
						sections={mode == 1 ? ringOutcomeData : ringIncomeData}
					/>
				</Grid.Col>
				<Grid.Col span={5}>
					<Table>
						<Table.Thead>
							<Table.Tr>
								<Table.Th>Category Name</Table.Th>
								<Table.Th>Total Income</Table.Th>
								<Table.Th>Total Outcome</Table.Th>
							</Table.Tr>
						</Table.Thead>
						<Table.Tbody>
							{categories.items?.map((c, index) => (
								<Table.Tr key={c.name || index}>
									<Table.Td>{c.name}</Table.Td>
									<Table.Td
										style={{
											color: 'green',
										}}
									>
										+ {c.categoryIncome} RON
									</Table.Td>
									<Table.Td
										style={{
											color: 'red',
										}}
									>
										- {c.categoryOutcome} RON
									</Table.Td>
								</Table.Tr>
							))}
						</Table.Tbody>
					</Table>
				</Grid.Col>
			</Grid>
		</Stack>
	);
};

export default TopCategories;
