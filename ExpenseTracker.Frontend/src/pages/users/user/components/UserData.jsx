import React from 'react';
import { useGetUserQuery } from '../../../../state/user/api';
import { useUnlockUserMutation } from '../../../../state/auth/api';
import { Avatar, Button, Group, Stack, Text } from '@mantine/core';
import { IconAt, IconJoinStraight, IconLock, IconLockOpen } from '@tabler/icons-react';

const UserData = ({ id }) => {
	const { data: user = {}, isLoading, isError } = useGetUserQuery(id);

	const [unlockUser, unlockUserResult] = useUnlockUserMutation();

	if (isLoading) return <>Loading...</>;

	return (
		<div>
			<Group wrap='nowrap'>
				<Avatar
					src='https://png.pngitem.com/pimgs/s/649-6490124_katie-notopoulos-katienotopoulos-i-write-about-tech-round.png'
					size={94}
					radius='md'
				/>
				<div>
					<Text fz='xs' tt='uppercase' fw={700} c='dimmed'>
						{user.username}
					</Text>

					<Text fz='lg' fw={500}>
						{user.firstName} {user.lastName}
					</Text>

					<Group wrap='nowrap' gap={10} mt={3}>
						<IconAt stroke={1.5} size='1rem' />
						<Text fz='xs' c='dimmed'>
							{user.email}
						</Text>
					</Group>

					<Group wrap='nowrap' gap={10} mt={5}>
						<IconJoinStraight stroke={1.5} size='1rem' />
						<Text fz='xs' c='dimmed'>
							Created at: {new Date(user.createdDateTime).toLocaleDateString('en-US')}
						</Text>
					</Group>
				</div>
				{user.lockedOut && (
					<>
						<Stack align='start' justify='center'>
							<Group align='center' justify='center'>
								<IconLock color='red' />
								<span
									style={{
										color: 'red',
									}}
								>
									User locked
								</span>
							</Group>
							<Button color='green' onClick={async () => await unlockUser(id)}>
								<IconLockOpen
									size={'17'}
									style={{
										marginInlineEnd: '.3rem',
									}}
								/>
								Unlock
							</Button>
						</Stack>
					</>
				)}
			</Group>
		</div>
	);
};

export default UserData;
