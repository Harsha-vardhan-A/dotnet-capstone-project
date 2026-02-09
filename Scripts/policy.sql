create table if not exists policy
(
    id  serial primary key,
    policy_name  varchar(50) not null,
    policy_description  text  not null,
    premium_amount numeric not null,
    created_at      timestamp        not null,
    updated_at      timestamp        not null,
    is_active       boolean     not null
);

insert into policy (policy_name, policy_description, premium_amount, created_at, updated_at, is_active) values
('Health Plus', 'Comprehensive health insurance plan covering hospitalization, surgeries, and outpatient treatments.', 5000.00, current_timestamp, current_timestamp, true),
('Auto Secure', 'Complete auto insurance plan covering accidents, theft, and third-party liabilities.', 3000.00, current_timestamp, current_timestamp, true),
('Home Safe', 'Home insurance plan covering fire, theft, and natural disasters.', 4000.00, current_timestamp, current_timestamp, false),
('Life Protect', 'Life insurance plan providing financial security for your loved ones.', 6000.00, current_timestamp, current_timestamp, true);

CREATE TABLE user_policy (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    policy_id INTEGER NOT NULL,
    status VARCHAR(250),
    requested_at TIMESTAMP,
    approved_at TIMESTAMP,
    CONSTRAINT fk_user_policy_user
        FOREIGN KEY (user_id)
        REFERENCES "user"(id)
        ON DELETE CASCADE,
    CONSTRAINT fk_user_policy_policy
        FOREIGN KEY (policy_id)
        REFERENCES policy(id)
        ON DELETE CASCADE
);

create table if not exists "user"
(
    id  serial primary key,
    name  varchar(250) not null,
    email varchar(225) not null unique,
    password_hash  varchar(250) not null,
    role      varchar(250) not null,
    created_at     timestamp,
    updated_at     timestamp
);