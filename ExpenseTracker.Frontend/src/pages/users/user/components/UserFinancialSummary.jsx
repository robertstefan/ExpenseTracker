import { Grid, SegmentedControl } from '@mantine/core';
import React, { useEffect, useState } from 'react';
import UserIncome from './UserIncome';
import UserOutcome from './UserOutcome';
import classes from '../styles/GradientSegmentControl.module.scss';
import { useGetRaportSummaryQuery } from '../../../../state/raport/api';
import { useRaportTimespan } from '../../../../context/RaportTimespanContext';
const UserFinancialSummary = ({ id }) => {
	const { raportTimespan, day, week, month, year, setRaportTimespan } = useRaportTimespan();

	const { data: raport = {}, isLoading, isError, refetch } = useGetRaportSummaryQuery({ userId: id, raportTimespan });

	useEffect(() => {
		refetch();
	}, [raportTimespan]);
	return (
		<>
			<SegmentedControl
				radius='xl'
				w={'100%'}
				data={[
					{ value: '1', label: 'Today' },
					{ value: '4', label: '1W' },
					{ value: '3', label: '1M' },
					{ value: '7', label: '6M' },
					{ value: '5', label: '1Y' },
				]}
				onChange={(e) => setRaportTimespan(e)}
				classNames={classes}
			/>
			<Grid.Col span={6}>
				<UserIncome income={raport.totalIncomme} isLoading={isLoading} />
			</Grid.Col>
			<Grid.Col span={6}>
				<UserOutcome outcome={raport.totalOutcome} isLoading={isLoading} />
			</Grid.Col>
		</>
	);
};

export default UserFinancialSummary;
