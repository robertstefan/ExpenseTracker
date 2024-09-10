import React from 'react';
import { Group, Paper, Skeleton, Text, ThemeIcon } from '@mantine/core';
import { IconArrowUpRight } from '@tabler/icons-react';
import CountUp from 'react-countup';

const UserIncome = ({ income, isLoading }) => {
	return (
		<Paper withBorder p='md' radius='md'>
			<Group justify='apart'>
				<div>
					<Text c='dimmed' tt='uppercase' fw={700} fz='xs'>
						Income
					</Text>
					{isLoading && income ? (
						<Skeleton height={20} radius='xl' />
					) : (
						<CountUp
							end={income}
							style={{
								fontSize: '1.25rem',
								fontWeight: '700',
							}}
							duration={1}
							separator='.'
							prefix='RON'
						/>
					)}
				</div>
				<ThemeIcon color='gray' variant='light' size={38} radius='md'>
					<IconArrowUpRight size='1.8rem' stroke={1.5} color='green' />
				</ThemeIcon>
			</Group>
		</Paper>
	);
};

export default UserIncome;
