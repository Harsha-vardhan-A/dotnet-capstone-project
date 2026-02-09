create table if not exists policy
(
    id  serial primary key,
    policy_name  varchar(50) not null,
    policy_description  text  not null,
    premium_amount numeric not null,
    created_at      date        not null,
    updated_at      date        not null,
    is_active       boolean     not null
);

insert into policy (policy_name, policy_description, premium_amount, created_at, updated_at, is_active) values
('Health Plus', 'Comprehensive health insurance plan covering hospitalization, surgeries, and outpatient treatments.', 5000.00, current_date, current_date, true),
('Auto Secure', 'Complete auto insurance plan covering accidents, theft, and third-party liabilities.', 3000.00, current_date, current_date, true),
('Home Safe', 'Home insurance plan covering fire, theft, and natural disasters.', 4000.00, current_date, current_date, false),
('Life Protect', 'Life insurance plan providing financial security for your loved ones.', 6000.00, current_date, current_date, true);